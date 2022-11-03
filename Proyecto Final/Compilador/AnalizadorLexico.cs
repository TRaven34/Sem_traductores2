namespace Analizador_Lexico__Traductor_
{
    public partial class Form1 : Form
    {
        //ATRIBUTOS GLOBALES//
        Lexico Lexico1 = new Lexico();
        OpenFileDialog OpenFile = new OpenFileDialog();   
        
        public Form1()
        {
            InitializeComponent();
        }
        //ANALIZAR//
        private void traducirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Entry = TBCode.Text;
            List<Token> LTokens = Lexico1.Scan(Entry);
            List<Variables> LVariables = Lexico1.GetVariables();
            List<Error> Warnings = new List<Error>();
            List<Error> Errors = new List<Error>(Lexico1.Errors);
            List<CalculadoraASM> LCalculadoraASM = new List<CalculadoraASM>(Lexico1.GetASM());
            DGSintaxis.DataSource = null;
            DGSintaxis.DataSource = LTokens;

            DGErrores.DataSource = null;
            DGErrores.DataSource = Errors;

            DGVariables.DataSource = null;
            DGVariables.DataSource = LVariables;

            DGASM.DataSource = null;
            DGASM.DataSource = LCalculadoraASM;

            for (int i = 0; i<LVariables.Count; i++)
            {

                if(LVariables[i].GetUsada()==false && LVariables[i].Tipos!= "Constante ensamblador" && LVariables[i].Tipos != "Registro ensamblador")
                {
                    Error NewWarning = new Error(LVariables[i].Identificador, "La variable esta declarada pero nunca se usa");
                    Warnings.Add(NewWarning);
                }
                if (LVariables[i].GetUsada() == false && LVariables[i].Tipos != "Constante ensamblador" && LVariables[i].Tipos != "Registro ensamblador")
                {
                    Error NewWarning = new Error(LVariables[i].Identificador, "La variable esta declarada pero nunca se usa");
                    Warnings.Add(NewWarning);
                }
            }

            for(int j = 0; j<LVariables.Count; j++)
            {
                Variables Var = LVariables[j];
                for(int i=j; i<LVariables.Count; i++)
                {
                    if(Var.Identificador==LVariables[i].Identificador)
                    {
                        Error NewWarning = new Error(LVariables[i].Identificador, "La variable esta declarada mas de una vez");
                        Errors.Add(NewWarning);
                    }
                }

            }

            DGWarnings.DataSource = null;
            DGWarnings.DataSource = Warnings;

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
                excel.Application.Workbooks.Add(true); //Con esto a�adimos una hoja en el Excel para exportar los archivos
                int IndiceColumna = 0;
                foreach (DataGridViewColumn columna in datalistado.Columns) //Aqu� empezamos a leer las columnas del listado a exportar
                {
                    IndiceColumna++;
                    excel.Cells[1, IndiceColumna] = columna.Name;
                }
                int IndiceFila = 0;
                foreach (DataGridViewRow fila in datalistado.Rows) //Aqu� leemos las filas de las columnas le�das
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

        //MOSTRAR OEPRACIONES ASM//
        private void operacionesASMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}