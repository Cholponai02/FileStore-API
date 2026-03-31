using Microsoft.AspNetCore.HttpOverrides;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace FileStore.Services
{
    public class SQLService
    {
        public byte[] fileKib { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string Pin { get; set; }
        public int pozn { get; set; }
        string conString = "Data Source=192.168.0.0; Initial Catalog=mainDB; User id=User_adm; Password=test;";
        string conStringOnlineDB = "Data Source=192.168.0.0; Initial Catalog=test_remote_identity; User id=User_adm; Password=test;";
        string conStrinLK = "Data Source=192.168.0.0; Initial Catalog=users; User id=User_adm; Password=test;";
        public SQLService CreateBibFileServer()
        {
            SQLService filess = new SQLService();
            List<SQLService> services = new List<SQLService>();
           
            using (SqlConnection sqlCon = new SqlConnection(conString))
            {
                string sqlQuery = $"select *from bib.BibDataClientPdf where FilePath is null";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SQLService file = new SQLService();
                        Files filesSaveDir = new Files();
                        TestFileService service = new TestFileService();
                        file.fileKib = (byte[])rdr["filePdf"];
                        file.fileName = (string)rdr["FileName"];
                        file.pozn = (int)rdr["IdNumber"];
                        file.Pin = (string)rdr["ClientINN"];
                        file.filePath = file.Pin + "\\" + "NumberProject\\" + file.pozn + "\\BIB\\" + file.fileName;
                        UpdateDatePdfKib(file.Pin, file.pozn,file.filePath);
                        filesSaveDir.PathFile = file.filePath;
                        filesSaveDir.FileData = file.fileKib;
                        service.SaveFiles(filesSaveDir);
                        services.Add(file);
                    }
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }
            return filess;
        }
        public void UpdateDatePdfBib(string pin, int pozn, string filePath)
        {
            using (SqlConnection sqlCon = new SqlConnection(conString))
            {
                string sqlQuery = $"update bib.BibDataClientPdf set FilePath ='{filePath}' where ClientINN='{pin}' and IdNumber={pozn}";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }
        }
        public SQLService CreditFileServer()
        {
            SQLService filess = new SQLService();
            List<SQLService> services = new List<SQLService>();

            using (SqlConnection sqlCon = new SqlConnection(conString))
            {
                string sqlQuery = $"select cd.id, kl.kl_number, cd.Name_pdf, cd.binary_pdf, cd.Number_Project from CreditDocuments cd , dogkr dg  , bankadm.klient kl where   dg.DG_KODKL=kl.kl_kod and cd.Number_Project=dg.Number_Project and cd.binary_pdf is not null";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SQLService file = new SQLService();
                        Files filesSaveDir = new Files();
                        TestFileService service = new TestFileService();
                        file.fileKib = (byte[])rdr["binary_pdf"];
                        file.fileName = (string)rdr["Name_pdf"];
                        file.pozn = (int)rdr["Number_Project"];
                        file.Pin = (string)rdr["kl_inn"];
                        file.filePath = file.Pin + "\\" + "Credits\\" + file.pozn + "\\Documents\\PDF\\" + file.fileName;
                        int idCred = (int)rdr["id"];
                        UpdateCreditPath(file.pozn, file.filePath, idCred);
                        filesSaveDir.PathFile = file.filePath;
                        filesSaveDir.FileData = file.fileKib;
                        service.SaveFiles(filesSaveDir);
                        services.Add(file);
                    }
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }
            return filess;
        }
        public void UpdateCreditPath(int number_prj, string filePath, int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(conString))
            {
                string sqlQuery = $"insert into CreditDocumentsCopy (Number_Project, Id_Credit, FilePath) values({number_prj},{id},'{filePath}')";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }
        }

        public SQLService PhotoNoPinSelfieClientFileServer()
        {
            SQLService filess = new SQLService();
            List<SQLService> services = new List<SQLService>();

            using (SqlConnection sqlCon = new SqlConnection(conString))
            {
                string sqlQuery = $"select zv.zv_inn, im.im_photo , im.id from image_klient im inner join zavkr zv on im.im_pozn=zv.ID_Proj where (im.inn is null) and im.im_photo is not null and zv.zv_inn<>''";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SQLService file = new SQLService();
                        Files filesSaveDir = new Files();
                        TestFileService service = new TestFileService();
                        file.fileKib = (byte[])rdr["im_photo"]; 
                        file.fileName = "SelfieClient.jpeg"; 
                        file.Pin = (string)rdr["zv_inn"]; 
                        int idPhoto = (int)rdr["id"]; 
                        file.filePath = file.Pin + "\\" + "Photos\\" + file.fileName;
                        UpdatePhotoOnlinePic1CreditPath(file.filePath, idPhoto);
                        filesSaveDir.PathFile = file.filePath;
                        filesSaveDir.FileData = file.fileKib;
                        service.SaveFiles(filesSaveDir);
                        services.Add(file);
                    }
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }
            return filess;
        }
 
        public SQLService fileOfferLKFileServer()
        {
            SQLService filess = new SQLService();
            List<SQLService> services = new List<SQLService>();

            using (SqlConnection sqlCon = new SqlConnection(conStrinLK))
            {
                string sqlQuery = $"select id , p_offer,kl_login from db.app_login where p_offer is not null";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SQLService file = new SQLService();
                        Files filesSaveDir = new Files();
                        TestFileService service = new TestFileService();
                        file.fileKib = (byte[])rdr["p_offer"];
                        file.fileName = "Publ_privacy" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".pdf";
                        file.Pin = (string)rdr["kl_login"];
                        int id = (int)rdr["id"];
                        file.filePath = file.Pin.Trim() + "\\MyApp\\PublicOffer\\" + file.fileName;
                        //UpdatePhotoOnlinePic1CreditPath(file.filePath, file.fileName, id);
                        //filesSaveDir.PathFile = file.filePath;
                        //filesSaveDir.FileData = file.fileKib;
                        //service.SaveFiles(filesSaveDir);
                        //services.Add(file);
                    }
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }
            return filess;
        }
        public void UpdatePhotoOnlinePic1CreditPath(string filePath, int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(conStringOnlineDB))
            {
                string sqlQuery = $"insert into SavedImgFPPath (IdPhoto, FrontPassportPath) values({id},'{filePath}')";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                sqlCon.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                sqlCon.Close();
            }

        }

    }
}
