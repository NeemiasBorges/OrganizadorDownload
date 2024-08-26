public class Worker : BackgroundService
{
    public ILogger<Worker> _logger;

    public static string _downloadsPath  = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public string _softwaresPath  = _downloadsPath + "\\" + "SOFTWARES" + "\\INSTALADORES";
    public string _documentsPath  = _downloadsPath + "\\" + "DOCUMENTOS";
    public string _imagesPath     = _downloadsPath + "\\" + "IMAGENS";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await OrganizeFiles();
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task OrganizeFiles()
    {
        try
        {
            Directory.CreateDirectory(_softwaresPath);
            Directory.CreateDirectory(_imagesPath);
            Directory.CreateDirectory(_documentsPath);

            var files = Directory.GetFiles(_downloadsPath);
            var movedFilesCount = 0;
            var failedFilesCount = 0;

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file).ToLower();

                if (extension == ".exe")
                {
                    if (MoveFile(file, _softwaresPath)) movedFilesCount++;
                    else failedFilesCount++;
                }
                else if (new[] { ".png", ".jpg", ".jpeg", ".gif" }.Contains(extension))
                {
                    if (MoveFile(file, _imagesPath)) movedFilesCount++;
                    else failedFilesCount++;
                }
                else if (new[] { ".pdf", ".docx", ".xlsx" }.Contains(extension))
                {
                    if (MoveFile(file, _documentsPath)) movedFilesCount++;
                    else failedFilesCount++;
                }
            }

            CleanEmptyDirectories();
            GenerateReport(movedFilesCount, failedFilesCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao organizar arquivos.");
        }
    }

    private bool MoveFile(string filePath, string destinationFolder)
    {
        try
        {
            var fileName = Path.GetFileName(filePath);
            var destinationPath = Path.Combine(destinationFolder, fileName);

            if (!File.Exists(destinationPath))
            {
                File.Move(filePath, destinationPath);
                _logger.LogInformation($"Movido: {filePath} para {destinationPath}");
                return true;
            }
            else
            {
                _logger.LogWarning($"Arquivo já existe: {destinationPath}");
                return false;
            }
        }
        catch (IOException ex) when (IsFileLocked(ex))
        {
            _logger.LogWarning($"Arquivo em uso: {filePath}");
            return false;
        }
    }

    private void CleanEmptyDirectories()
    {
        foreach (var directory in new[] { _softwaresPath, _imagesPath, _documentsPath })
        {
            try
            {
                if (!Directory.GetFiles(directory).Any() && !Directory.GetDirectories(directory).Any())
                {
                    Directory.Delete(directory);
                    _logger.LogInformation($"Diretório vazio deletado: {directory}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, $"Permissões insuficientes para deletar diretório: {directory}");
            }
        }
    }

    private void GenerateReport(int movedFiles, int failedFiles)
    {
    }

    private bool IsFileLocked(IOException exception)
    {
        int errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(exception) & ((1 << 16) - 1);
        return errorCode == 32 || errorCode == 33;
    }
}
