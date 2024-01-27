using LMS.Business.DTOs;

namespace LMS.UI.Desktop.Forms
{
    public partial class frmBooks : Form
    {
        public UserDTO currentUser;
        public frmBooks()
        {
            InitializeComponent();
        }

        private void frmBooks_Load(object sender, EventArgs e)
        {
            lblCurrentUser.Text = currentUser?.Name;
        }
    }
}
