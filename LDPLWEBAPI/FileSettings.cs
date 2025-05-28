namespace LDPLWEBAPI
{
    public class FileSettings
    {
        public string DAFilesRootPath { get; set; }

        public string EPunchFilesRootPath { get; set; }

        public int MaxUploadSize { get; set; }

        public int MaxBillsUpload { get; set; }
    }
}
