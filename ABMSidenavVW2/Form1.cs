using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq.Expressions;

namespace ABMSidenavVW2
{
    
    public partial class Form1 : Form
    {
        bool modifico = false;
        public Form1()
        {
            InitializeComponent();
            cboUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
            TreeVMenu.AfterCheck += TreeVMenu_AfterCheck;
            btnGuardar.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                cargarTreeView(TreeVMenu);
                CargarCombos("UsuarioVul", "usuario_id", cboUsuarios);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void cargarTreeView(System.Windows.Forms.TreeView oTreeView)
        {
            try
            {
                QueryExecutor executor = new QueryExecutor();


                executor.ConectDBreader("select itemmenu_id, itempadre_id, sCaption from ItemMenu\r\nwhere tipomenu_id = 7\r\norder by itempadre_id,nOrden");
                // Código para ejecutar la consulta y asignar los resultados al ComboBox
                

                List<Item> items = new List<Item>();

                while (executor.Reader.Read())
                {
                    int itemmenu_id = Convert.ToInt32(executor.Reader["itemmenu_id"].ToString());
                    int itempadre_id = Convert.ToInt32(executor.Reader["itempadre_id"].ToString());
                    string sCaption = executor.Reader.GetString(2);

                    Item item = new Item(itemmenu_id, itempadre_id, sCaption);
                    items.Add(item);
                }

                executor.desconectar();

                oTreeView.Nodes.Clear();

                // Agregar los nodos raíz que no tienen un número de ítem padre
                foreach (Item item in items)
                {
                    if (item.ItemPadre == 0)
                    {
                        TreeNode nodo = new TreeNode(item.Nombre);
                        nodo.Tag = item;
                        oTreeView.Nodes.Add(nodo);
                    }
                }

                // Agregar los nodos hijos recursivamente
                AgregarNodosHijos(oTreeView.Nodes, items);

                

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un error al cargar el TreeView: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar el TreeView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void AgregarNodosHijos(TreeNodeCollection nodos, List<Item> items)
        {
            try
            {

                foreach (TreeNode nodoPadre in nodos)
                {
                    Item itemPadre = (Item)nodoPadre.Tag;
                    foreach (Item item in items)
                    {
                        if (item.ItemPadre == itemPadre.ItemNumero)
                        {
                            TreeNode nodoHijo = new TreeNode(item.Nombre);
                            nodoHijo.Tag = item;
                            nodoPadre.Nodes.Add(nodoHijo);
                        }
                    }

                    // Llamar recursivamente al método para agregar los nodos hijos del nodo actual
                    AgregarNodosHijos(nodoPadre.Nodes, items);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al agregar Nodos Hijos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void TreeVMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void TreeVMenu_AfterCheck(object sender, TreeViewEventArgs e)
        {
                
                if (e.Action != TreeViewAction.Unknown)
                {
                    btnGuardar.Enabled = true;
                    // Verificar si se seleccionó un nodo padre
                    if (e.Node.Nodes.Count > 0)
                    {
                        PropagarSeleccionHijos(e.Node, e.Node.Checked);
                    }
                }
        }

        private void PropagarSeleccionHijos(TreeNode parentNode, bool isChecked)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = isChecked;
                if (childNode.Nodes.Count > 0)
                {
                    PropagarSeleccionHijos(childNode, isChecked);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = false;
            if (cboUsuarios.SelectedValue.ToString() != null)
            {
                actualizarTreeView(TreeVMenu, cboUsuarios.SelectedValue.ToString());
            }
        }


        private void CargarCombos(string nombreTabla, string columna, System.Windows.Forms.ComboBox cbo)
        {

            QueryExecutor executor = new QueryExecutor();

            


            DataTable dataTable = executor.ConectDBdt("select * from " + nombreTabla); //creo una tabla de alto nivel en memoria
            
            cbo.DataSource = dataTable; //le asigno la tabla en memoria al darasource del objeto(establece el origen de los datos del objeto

            cbo.DisplayMember = dataTable.Columns[0].ColumnName; //dato que se muestra
            cbo.ValueMember = dataTable.Columns[0].ColumnName;  //dato que se guarda


            //while (reader.Read())
            //        {
            //            // Lee los datos y agrega los elementos al ComboBox
            //            cbo.Items.Add(reader.GetString(0));
            //        }





        }


        public void actualizarTreeView(System.Windows.Forms.TreeView oTree, string usuario)
        {


            QueryExecutor executor = new QueryExecutor();

            //usuario = "administrador";

            // Código para ejecutar la consulta y asignar los resultados al ComboBox
            SqlDataReader reader = executor.ExecuteQuery("select   usu.itemmenu_id as ITEM from UsuarioPerfil usu with(nolock)\r\nleft join ItemMenu it with(nolock) on usu.itemmenu_id = it.itemmenu_id\r\nwhere it.tipomenu_id = 7 and usuario_id = '" + usuario + "'");


            executor.ConectDBreader("select   usu.itemmenu_id as ITEM from UsuarioPerfil usu with(nolock) " +
                "left join ItemMenu it with(nolock) on usu.itemmenu_id = it.itemmenu_id " +
                "where it.tipomenu_id = 7 and usuario_id = '" + usuario + "'");
            

            List<int> items = new List<int>();

            while (executor.Reader.Read())
            {

                items.Add(Convert.ToInt32(executor.Reader["ITEM"].ToString()));
            }

            executor.desconectar();


            MarcarCheckboxPadres(oTree, items);


        }

        private void MarcarCheckboxPadres(System.Windows.Forms.TreeView nodos, List<int> items)
        {
            foreach (TreeNode nodo in nodos.Nodes)
            {
                // Verificar si el nodo cumple con la condición en el Tag
                if (nodo.Tag is Item i && items.Contains(i.ItemNumero))
                {
                    nodo.Checked = true;
                } else nodo.Checked = false;

                // Llamar recursivamente al método para los nodos hijos
                MarcarheckboxHijos(nodo, items);
            }



        }

        private void MarcarheckboxHijos(System.Windows.Forms.TreeNode nodos, List<int> items)
        {
            foreach (TreeNode nodo in nodos.Nodes)
            {
                // Verificar si el nodo cumple con la condición en el Tag
                if (nodo.Tag is Item i && items.Contains(i.ItemNumero))
                {
                    nodo.Checked = true;
                }
                else nodo.Checked = false;

                // Llamar recursivamente al método para los nodos hijos
                MarcarheckboxHijos(nodo, items);
            }



        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Seguro que desea grabar los cambios?", "GRABAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {

                try
                {

                    List<Item> Checkeditems = new List<Item>();
                    Checkeditems = CheckedItems(TreeVMenu);
                    save(Checkeditems, cboUsuarios.SelectedValue.ToString());
                    actualizarTreeView(TreeVMenu, cboUsuarios.SelectedValue.ToString());
                    MessageBox.Show("Cambios guardados con éxito", "Cambios guardados", MessageBoxButtons.OK, MessageBoxIcon.None);
                    btnGuardar.Enabled = false;


                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            

        }

        private List<Item> CheckedItems(System.Windows.Forms.TreeView oTree)
        {
            List<Item> list = new List<Item>();

            foreach(TreeNode nodo in oTree.Nodes)
            {
                if(nodo.Checked )  { list.Add((Item)nodo.Tag); };
                list.AddRange(CheckedChildItems(nodo));
            }
            return list;
        }

        private List<Item> CheckedChildItems(TreeNode Nodes)
        {
            List<Item> list = new List<Item>();

            foreach (TreeNode nodo in Nodes.Nodes)
            {
                if (nodo.Checked) { list.Add((Item)nodo.Tag); };

            }
            return list;
        }

        private void save(List<Item> items, string usuario)
        {
            QueryExecutor queryExcutor = new QueryExecutor();
            SqlDataReader reader = queryExcutor.ExecuteQuery($"delete  UsuarioPerfil from UsuarioPerfil usu " +
                $"join ItemMenu it on usu.itemmenu_id = it.itemmenu_id " +
                $"where usu.usuario_id = '{usuario}' and it.tipomenu_id =7");
          

            foreach (Item item in items)
            {
                reader = queryExcutor.ExecuteQuery($"insert into UsuarioPerfil (usuario_id, itemmenu_id) values('{usuario}', {item.ItemNumero.ToString()})");
            }

        }
    }
}
