using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace FileStore.Helpers;

public class ReplacingWordsHelper
{
    public void ReplaceWords(string swhat, string sbywhat, WordprocessingDocument wordDoc)
    {
        int aaa = 1;
        var body = wordDoc.MainDocumentPart.Document.Body;
        foreach (var para in body.Elements<Paragraph>())
        {
            string dddd = para.InnerText;
            string szam1 = swhat;
            int len1 = swhat.Length;
            int pos1 = dddd.IndexOf(swhat);
            int pos2 = pos1 + len1;
            if (pos1 >= 0)
            {


                Regex regexText = new Regex(szam1);
                RunProperties bb = null; //new RunProperties();
                {
                    int obrab = 0;
                    if (dddd.IndexOf("<c1>") >= 0 && swhat.IndexOf("<c1>") >= 0)
                        aaa = 3;
                    // если целиком
                    foreach (var r in para.Elements<Run>())
                    {
                        string ddd3 = r.InnerText;
                        if (ddd3.IndexOf("<c1>") >= 0 && swhat.IndexOf("<c1>") >= 0)
                            aaa = 3;
                        foreach (var t in r.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>())
                        {
                            string dddd1 = t.Text;
                            if (dddd1.IndexOf("<c1>") >= 0 && swhat.IndexOf("<c1>") >= 0)
                                aaa = 3;

                            if (dddd1.Contains(swhat))
                            {
                                dddd1 = regexText.Replace(dddd1, sbywhat.ToString().Trim());
                                t.Text = dddd1;
                                obrab = 1;
                            }
                        }
                    }


                    if (obrab == 0)
                    {
                        // 1-ый прожод
                        int pos_cur = 0;
                        string sbywhat1 = sbywhat;
                        foreach (var r in para.Elements<Run>())
                        {
                            string ddd3 = r.InnerText;

                            if (ddd3.IndexOf("<c1>") >= 0 && swhat.IndexOf("<c1>") >= 0)
                                aaa = 3;
                            foreach (var t in r.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>())
                            {
                                string dddd1 = t.Text;
                                pos_cur = pos_cur + dddd1.Length;
                                if (pos_cur > pos1 && pos_cur <= pos2)
                                {
                                    //dddd1 = regexText.Replace(dddd1, sbywhat1.ToString().Trim());
                                    dddd1 = sbywhat1.ToString().Trim();
                                    t.Text = dddd1;
                                    sbywhat1 = "";
                                }
                            }
                        }

                    }
                }
            }
        }
        foreach (var tt in wordDoc.MainDocumentPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>())
        {
            var table = tt;
            foreach (var row in table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
            {
                foreach (var cell in row.Elements<TableCell>())
                {
                    foreach (var p in cell.Elements<Paragraph>())
                    {
                        string dddd = p.InnerText;
                        RunProperties bb = null; //new RunProperties();
                        if (dddd.IndexOf("<c1>") >= 0 && swhat.IndexOf("<c1>") >= 0)
                            aaa = 3;
                        {
                            aaa = 3;
                            foreach (var r in p.Elements<Run>())
                            {
                                var aa = r.GetAttributes();
                                bb = r.RunProperties;
                                //var mycolor=bb.Color;
                            }
                        }
                        Regex regexText = new Regex(swhat);
                        if (dddd.IndexOf(swhat) >= 0)
                        {
                            dddd = regexText.Replace(dddd, sbywhat.ToString().Trim());

                            p.RemoveAllChildren<Run>();

                            Run run = p.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            if (bb != null)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.Color color = new DocumentFormat.OpenXml.Wordprocessing.Color();
                                if (bb.Color != null)
                                {
                                    color.Val = bb.Color.Val;
                                    runProperties.AppendChild(color);
                                }
                            }
                            run.AppendChild(new RunProperties(new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new DocumentFormat.OpenXml.Wordprocessing.Text(dddd)));
                        }


                    }
                }
            }
        }


    }
}
