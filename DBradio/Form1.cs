using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DBradio
{
    public class Coord
    {
        public double Y { get; set; }
        public double X { get; set; }
    }

    public class XXX
    {
        public String TypeAntenne { get; set; }
        public String Proprietaire { get; set; }
        public String NatureSupport { get; set; }
        public String Exploitant { get; set; }

        public Coord Coord { get; set; }
        public String AdresseLieu { get; set; }
        public String Adresse1 { get; set; }
        public String Adresse2 { get; set; }
        public String Adresse3 { get; set; }

        public DateTime DateImplantation { get; set; }
        public DateTime DateModif { get; set; }
        public DateTime DatePremierService { get; set; }
        public DateTime DateServiceEmetteur { get; set; }

        public Single Dimension { get; set; }
        public String TypeRayonnement { get; set; }
        public Single Azimut { get; set; }
        public Single HauteurSol { get; set; }

        public Single DebFrq { get; set; }
        public Single FinFrq { get; set; }
        public String UniteFrq { get; set; }
        public String TypeEmission { get; set; }
    }

    public partial class Form1 : Form
    {
        public SQLiteConnection con;
        public const String dbname = "DBradio.sqlite";

        //import par lot de xxxxx iterations
        //reset le datagridview

        //requete utiles :
        //créer un KML pour un set d'antenne

        public Form1()
        {
            InitializeComponent();

            if (!File.Exists(dbname))
                SQLiteConnection.CreateFile(dbname);

            con = new SQLiteConnection("URI=file:" + dbname);
            con.Open();
        }


        public void CreateTables()
        {
            String sql = File.ReadAllText("import/script.sql");

            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public void Import(String filename)
        {
            StringBuilder sb = new StringBuilder();
            String line;
            StreamReader sr = new StreamReader("import/" + filename, Encoding.UTF8);
            int i = 0, nbIteration = 1000;

            sr.ReadLine(); //saute la première ligne
            while ((line = sr.ReadLine()) != null)
            {
                String[] fields = line.Split(new char[] { ';' });
                sb.Append("(\"" + String.Join("\",\"", fields) + "\"),");

                if (i++ == nbIteration)
                {
                    break;
                }
            }

            sb.Remove(sb.Length - 1, 1);
            Insert(filename, sb.ToString());
        }

        public void Insert(String filename, String sql)
        {
            String table = filename.Substring(4, filename.Length - 8);
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + table + " VALUES " + sql, con);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(table + " : " + e.Message);
            }
        }

        public void xxx()
        {
            //filtres

            //coordonnées
            String y = "", x = "";
            int rayon = 5;//km

            String date = ""; //date intall + date MES

            String typeEmetteur = ""; //PMR, UTML, FM, etc

            String frq = "";
            String uniteFrq = "";

            String sql = "SELECT TAE_LB as type_antenne, TPO_LB as proprietaire, NAT_LB_NOM as nature, ADM_LB_NOM as exploitant, " + //label par jointures
                        "COR_NB_DG_LAT, COR_NB_MN_LAT, COR_NB_SC_LAT, COR_CD_NS_LAT, COR_NB_DG_LON, COR_NB_MN_LON, COR_NB_SC_LON, COR_CD_EW_LON, " + //coordonnés
                        "ADR_LB_LIEU, ADR_LB_ADD1, ADR_LB_ADD2, ADR_LB_ADD3, Dte_Implantation,Dte_modif,Dte_En_Service, EMR_DT_SERVICE, " + //adresses + dates
                        "AER_NB_DIMENSION,AER_FG_RAYON,AER_NB_AZIMUT,AER_NB_ALT_BAS, " + //dimension, type rayonnement, azm, hauteur par rapport au sol
                        "BAN_NB_F_DEB, BAN_NB_F_FIN, BAN_FG_UNITE, EMR_LB_SYSTEME " + //FRQ
                        "FROM SUPPORT SUP " +
                        "INNER JOIN STATION STA ON STA.STA_NM_ANFR = SUP.STA_NM_ANFR " +
                        "INNER JOIN ANTENNE ANT ON ANT.STA_NM_ANFR = STA.STA_NM_ANFR " +
                        "INNER JOIN EMETTEUR EME ON EME.AER_ID = ANT.AER_ID " +
                        "INNER JOIN BANDE BAN ON BAN.EMR_ID = EME.EMR_ID " +

                        "INNER JOIN EXPLOITANT EXP ON EXP.ADM_ID = STA.ADM_ID " +
                        "INNER JOIN TYPE_ANTENNE TAN ON TAN.TAE_ID = ANT.TAE_ID " +
                        "INNER JOIN PROPRIETAIRE PRO ON PRO.TPO_ID = SUP.TPO_ID " +
                        "INNER JOIN NATURE NAT ON NAT.NAT_ID = SUP.NAT_ID " +
                        "WHERE 1 ";

            //coordonnées
            if (!String.IsNullOrEmpty(y))
            {
                //création de carré 

                sql += " ";
            }


            //critère : a partir de, jusqu'a, entre x et y
            if (!String.IsNullOrEmpty(date))
            {
                sql += "AND (Dte_En_Service = '" + date + "' OR EMR_DT_SERVICE= '" + date + "') ";
            }

            //liste, frq min, frq max
            if (!String.IsNullOrEmpty(frq))
            {
                sql += "AND (BAN_NB_F_DEB <= " + frq + " AND " + frq + " => BAN_NB_F_FIN AND BAN_FG_UNITE = " + uniteFrq + " ) ";
            }

            //liste
            if (!String.IsNullOrEmpty(typeEmetteur))
            {
                sql += "AND EMR_LB_SYSTEME = " + typeEmetteur + " ";
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    tab[i] = rdr.GetValue(i);
                }
            }

        }

        public int BuildKML(List<XXX> listXXX)
        {
            StringBuilder sb = new StringBuilder();

            String xmlheader = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><kml>" +
                                "<Document>";
            String xmlfooter = "</Folder></Document></kml>";

            String tplLigne = "<Placemark>" + Environment.NewLine +
                                "<name>{nom}</name>" + Environment.NewLine +
                                "<description>{description}</description>" + Environment.NewLine +
                                "<styleUrl>#{style}</styleUrl>" + Environment.NewLine +
                                "<Point>" + Environment.NewLine +
                                "<altitudeMode>clampToGround</altitudeMode>" + Environment.NewLine +
                                "<extrude>0</extrude>" + Environment.NewLine +
                                "<coordinates>{maloc}</coordinates>" + Environment.NewLine +
                                "</Point>" + Environment.NewLine +
                                "</Placemark>";

            sb.AppendLine(xmlheader);
            //style
            sb.AppendLine("<Style id=\"point-1\"></Style>");

            //ouverture du dossier lignes
            sb.AppendLine("<Folder>" + Environment.NewLine +
                            "<name>Point Features</name>" +
                            "<description>Point Features</description>");


            /*
            
            <Placemark>
              <description><![CDATA[Unknown Point Feature<BR><BR><B>attr1</B> = supervalue1<BR><B>hateur</B> = 4556]]></description>
              <name>lmlmlm</name>
              <styleUrl>#point1</styleUrl>
              <Point>
                <altitudeMode>clampToGround</altitudeMode>
                <extrude>0</extrude>
                <coordinates>7.7319889632,48.8566529776,-999999</coordinates>
              </Point>
            </Placemark>

            */
            int nbTirs = 0;

            for (int i = 0; i < listXXX.Count; i++)
            {
                String nom = "";
                String mydesc = "<![CDATA[[Unknown Point Feature<BR><BR>" +
                                "<B>DebFrq</B> = " + listXXX[i].DebFrq + "<BR>" +
                                "<B>FinFrq</B> = " + listXXX[i].FinFrq + "<BR>" +
                                "<B>UniteFrq</B> = " + listXXX[i].UniteFrq + "<BR>" +

                                "<B>Adresse1</B> = " + listXXX[i].Adresse1 + "<BR>" +
                                "<B>Adresse2</B> = " + listXXX[i].Adresse2 + "<BR>" +
                                "<B>Adresse3</B> = " + listXXX[i].Adresse3 + "<BR>" +
                                "<B>AdresseLieu</B> = " + listXXX[i].AdresseLieu + "<BR>" +

                                "<B>Azimut</B> = " + listXXX[i].Azimut + "<BR>" +
                                "<B>TypeRayonnement</B> = " + listXXX[i].TypeRayonnement + "<BR>" +
                                "<B>TypeEmission</B> = " + listXXX[i].TypeEmission + "<BR>" +
                                "<B>TypeAntenne</B> = " + listXXX[i].TypeAntenne+ "<BR>" +
                                
                                "<B>Azimut</B> = " + listXXX[i].Exploitant + "<BR>" +
                                "<B>Azimut</B> = " + listXXX[i].Proprietaire + "<BR>" +
                                "<B>Azimut</B> = " + listXXX[i].NatureSupport + "<BR>" +

                                "<B>Dimension</B> = " + listXXX[i].Dimension + "<BR>" +
                                "<B>HauteurSol</B> = " + listXXX[i].HauteurSol + "<BR>" +

                                "<B>DateImplantation</B> = " + listXXX[i].DateImplantation + "<BR>" +
                                "<B>DateModif</B> = " + listXXX[i].DateModif + "<BR>" +
                                "<B>DatePremierService</B> = " + listXXX[i].DatePremierService + "<BR>" +
                                "<B>DateServiceEmetteur</B> = " + listXXX[i].DateServiceEmetteur + "<BR>" +
                                
                                "]]>";

                //nom / desc / locs du tir
                String tmpLigne = tplLigne;

                tmpLigne = tmpLigne.Replace("{nom}", nom);
                tmpLigne = tmpLigne.Replace("{description}", mydesc);
                tmpLigne = tmpLigne.Replace("{style}", "point-1");

                //Dans le KML c'est X;Y;ALT
                String loc = listXXX[i].Coord.X.ToString().Replace(",", ".") + "," +
                            listXXX[i].Coord.Y.ToString().Replace(",", ".") + ",0";

                tmpLigne = tmpLigne.Replace("{maloc}", loc);

                sb.AppendLine(tmpLigne);
            }

            sb.AppendLine(xmlfooter);
            WriteFile(sb.ToString(), "releves_geo.kml");

            return nbTirs;
        }


        private void b_import_Click(object sender, EventArgs e)
        {
            Import("SUP_ANTENNE.txt");
            tb_log.AppendText("SUP_ANTENNE importé" + Environment.NewLine);
            Import("SUP_BANDE.txt");
            tb_log.AppendText("SUP_BANDE importé" + Environment.NewLine);
            Import("SUP_EMETTEUR.txt");
            tb_log.AppendText("SUP_EMETTEUR importé" + Environment.NewLine);
            Import("SUP_EXPLOITANT.txt");
            tb_log.AppendText("SUP_EXPLOITANT importé" + Environment.NewLine);
            Import("SUP_NATURE.txt");
            tb_log.AppendText("SUP_NATURE importé" + Environment.NewLine);
            Import("SUP_PROPRIETAIRE.txt");
            tb_log.AppendText("SUP_PROPRIETAIRE importé" + Environment.NewLine);
            Import("SUP_STATION.txt");
            tb_log.AppendText("SUP_STATION importé" + Environment.NewLine);
            Import("SUP_SUPPORT.txt");
            tb_log.AppendText("SUP_SUPPORT importé" + Environment.NewLine);
            Import("SUP_TYPE_ANTENNE.txt");
            tb_log.AppendText("SUP_TYPE_ANTENNE importé" + Environment.NewLine);
        }

        private void b_createTable_Click(object sender, EventArgs e)
        {
            CreateTables();
            tb_log.AppendText("Tables crées" + Environment.NewLine);
        }

        private void b_execSQL_Click(object sender, EventArgs e)
        {
            String sql = tb_sql.Text;
            SQLiteCommand cmd = new SQLiteCommand(sql, con);

            try
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    Object[] tab = new Object[rdr.FieldCount];
                    for (int i = 0; i < tab.Length; i++) dataGridView1.Columns.Add(rdr.GetName(i), rdr.GetName(i));

                    while (rdr.Read())
                    {
                        for (int i = 0; i < tab.Length; i++) tab[i] = rdr.GetValue(i);
                        dataGridView1.Rows.Add(tab);
                    }
                }
            }
            catch (Exception ex)
            {
                tb_log.AppendText(ex.Message + Environment.NewLine);
            }
        }

        public void WriteFile(String txt, String filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252)))
            {
                sw.Write(txt);
            }
        }
    }
}
