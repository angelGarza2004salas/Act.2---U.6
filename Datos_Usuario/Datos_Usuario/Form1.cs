using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datos_Usuario
{
    public partial class Form1 : Form
    {
        // estoy utilizando clases que me permiten escribir un archivo binario y leerlo (binaryreader y binarywriter)
        // ademas de utilizar filestream para manejar los archivos que se han creado o que se vayan a utilizar en la aplicacion
        // y un openfiledialog que me ayuda a manejar la ruta de un archivo existente
        private BinaryReader lectorDeBinarios;
        private BinaryWriter escritorDeBinarios;
        private FileStream manejoDeArchivos;
        private OpenFileDialog rutaAbierta;
        private SaveFileDialog rutaGuardada;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // como vimos en clase detecto el error en lugar de esperar a que suceda ese error
            // aqui si no se llena alguno de los datos del formulario no permito que se cree un archivo
            // porque no tendria datos que meter en ese archivo
            if (txtMaterno == null || txtNombre == null || txtPaterno == null)
            {
                MessageBox.Show("Error al escoger la ruta de archivo. ", "Error", MessageBoxButtons.OK);
                return;
            }     

            // ruta seleccionada por el usuario en la que se guardara el archivo
            rutaGuardada = new SaveFileDialog();
            rutaGuardada.Filter = "Archivos binarios (*.bin)|*.bin";
            rutaGuardada.Title = "Datos del usuario";

            // como vimos en clase detecto el error en lugar de esperar a que suceda ese error
            // aqui si no se escogiera una ruta no se crearia el archivo porque no le indicariamos que escribir en el
            if (rutaGuardada.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("Error al escoger la ruta de archivo. ", "Error", MessageBoxButtons.OK);
                return;
            }

            // relleno los parametros de la clase manejadora de archivos (filestream)
            // en el parametro 1 le entrego la ruta que escogi con el showdialog, en el 2 indico que quiero crear el archivo 
            // y en el tercero indico que quiero escribir en el archivo
            manejoDeArchivos = new FileStream(rutaGuardada.FileName, FileMode.Create, FileAccess.Write);
            escritorDeBinarios = new BinaryWriter(manejoDeArchivos);

            // con el escritor de archivos (binarywriter) escribo el archivo
            escritorDeBinarios.Write(txtNombre.Text + " " + txtPaterno.Text + " " + txtMaterno.Text + " " + nudEdad.Value.ToString());
            
            // cierro o termino la escritura y el manejo de los archivos despues de leerlos
            escritorDeBinarios.Close();
            manejoDeArchivos.Close();

            // despues de escribir los datos y crear el archivo muestro una ventana emergente con los datos del usuario
            // ingresados en esa sesion
            MessageBox.Show("Tus datos son: " + txtNombre.Text + " " + txtPaterno.Text + " " + txtMaterno.Text + " " + nudEdad.Text, "Datos del usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Tu archivo con estos datos se guardo correctamente en la ruta seleccionada", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            // filtrando los archivos (txt, dll, etc) para ver unicamente los archivos binarios que hay en la computadora
            rutaAbierta = new OpenFileDialog();
            rutaAbierta.FileName = "Datos de usuario";
            rutaAbierta.Filter = "Archivos de binarios (*.bin)|*.bin";

            if (rutaAbierta.ShowDialog() == DialogResult.OK)
            {
                // relleno los parametros de la clase manejadora de archivos (filestream)
                // en el parametro 1 le entrego la ruta que escogi con el showdialog, en el 2 indico que quiero abrir el archivo 
                // y en el tercero indico que quiero leer el archivo
                manejoDeArchivos = new FileStream(rutaAbierta.FileName, FileMode.Open, FileAccess.Read);
                lectorDeBinarios = new BinaryReader(manejoDeArchivos);

                // guardo la linea de datos leida´por el lector de archivos al que le indico leer datos de tipo string
                string datosDeUsuario = lectorDeBinarios.ReadString();

                // usando la variable datosDeUsuario muestro los datos leidos en el archivo seleccionado
                MessageBox.Show(datosDeUsuario, "Datos de un usuario anterior", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Puede seguir creando mas archivos", "Puede continuar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lectorDeBinarios.Close();
                manejoDeArchivos.Close();
            }
            else
            {
                MessageBox.Show("No se pudo leer el archivo", "Ocurrio un problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // cierro o termino la lectura y el manejo de los archivos despues de leerlos
          
        }
    }
}
