using System.Windows;
using System.Windows.Controls;
using SolrEngine;
using SolrEngine.Helpers;
using SqlData.Context;

namespace StudioWPF.Configuration
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        private readonly AppDbContext _context;
        private readonly ISolrManager _solrManager;

        public ConfigurationPage(AppDbContext context, ISolrManager solrManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _solrManager = solrManager ?? throw new ArgumentNullException(nameof(solrManager));
            InitializeComponent();
            SetParamsValue();
        }

        private void SetParamsValue()
        {
            txtSolrUrl.Text = _context.AppParameters.FirstOrDefault(x => x.Name == "SolrUrl")?.Value;
            txtSolrUser.Text = _context.AppParameters.FirstOrDefault(x => x.Name == "SolrLogin")?.Value;
            var cipherText = _context.AppParameters.FirstOrDefault(x => x.Name == "SolrPassword")
                ?.Value;
            if (cipherText != null)
                txtSolrPass.Password =
                    EncryptionManager.Decrypt(cipherText);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Changing this parameters require application restart. Do you wish to proceed?", "Warning",MessageBoxButton.YesNo,MessageBoxImage.Warning, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var url = _context.AppParameters.First(x => x.Name == "SolrUrl");
                    var login = _context.AppParameters.First(x => x.Name == "SolrLogin");
                    var password = _context.AppParameters.First(x => x.Name == "SolrPassword");

                    url.Value = txtSolrUrl.Text;
                    login.Value = txtSolrUser.Text;
                    password.Value = EncryptionManager.Encrypt(txtSolrPass.Password);
                    _context.SaveChanges();

                    System.Windows.Forms.Application.Restart();
                    System.Windows.Application.Current.Shutdown();
                }
                catch(InvalidOperationException ex)
                {
                    MessageBox.Show($"There is no suitable param in Database. Please upgrade your database: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void IndexSolr_Click(object sender, RoutedEventArgs e)
        {
            var result = _solrManager.IndexElements(_context.Products.ToArray());
            if (result)
                MessageBox.Show("Indexation completed successfully!", "Finish", MessageBoxButton.OK,
                    MessageBoxImage.Information);
        }
    }
}
