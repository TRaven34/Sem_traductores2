namespace Analizador_Lexico__Traductor_
{
    public partial class Form1 : Form
    {
        //ATRIBUTOS GLOBALES//
        Lexico Lexico1 = new Lexico();
        OpenFileDialog OpenFile = new OpenFileDialog();
        List<Token> LSTToken = new List<Token>();
        public Form1()
        {
            InitializeComponent();
        }
        //ANALIZAR//
        private void traducirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Entry = TBCode.Text;
            List<Token> LTokens = Lexico1.Scan(Entry);
            DGSintaxis.DataSource = null;
            DGSintaxis.DataSource = LTokens;
            DGErrores.DataSource = null;
            DGErrores.DataSource = Lexico1.Errors;
        }

        //LIMPIAR//
        private void limpiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DGSintaxis.DataSource = null;
            TBCode.Text = "";
        }

        //ABRIR TXT//
        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(OpenFile.ShowDialog()==DialogResult.OK)
            {
                TBCode.Text = File.ReadAllText(OpenFile.FileName);
            }
        }

        //GUARDAR//
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportarDatos(DGSintaxis);
        }

        private void ExportarDatos(DataGridView datalistado)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application(); // Instancia a la libreria de Microsoft Office
                excel.Application.Workbooks.Add(true); //Con esto añadimos una hoja en el Excel para exportar los archivos
                int IndiceColumna = 0;
                foreach (DataGridViewColumn columna in datalistado.Columns) //Aquí empezamos a leer las columnas del listado a exportar
                {
                    IndiceColumna++;
                    excel.Cells[1, IndiceColumna] = columna.Name;
                }
                int IndiceFila = 0;
                foreach (DataGridViewRow fila in datalistado.Rows) //Aquí leemos las filas de las columnas leídas
                {
                    IndiceFila++;
                    IndiceColumna = 0;
                    foreach (DataGridViewColumn columna in datalistado.Columns)
                    {
                        IndiceColumna++;
                        excel.Cells[IndiceFila + 1, IndiceColumna] = fila.Cells[columna.Name].Value;
                    }
                }
                excel.Visible = true;
            }
            catch (Exception)
            {
                MessageBox.Show("No hay Registros a Exportar.");
            }
        }
    }
}