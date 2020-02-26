using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreDbSpCallMVC.Models.DB
{
    public partial class db_core_sp_callContext : DbContext
    {
        public db_core_sp_callContext()
        {
        }

        public db_core_sp_callContext(DbContextOptions<db_core_sp_callContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblDepartment> TblDepartment { get; set; }
        public virtual DbSet<TblProduct> TblProduct { get; set; }
        public virtual DbSet<TblVendor> TblVendor { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=db_core_sp_call;Trusted_Connection=True;");
        //            }
        //        }

        [Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.ToTable("tbl_department");

                entity.Property(e => e.GroupName).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("tbl_product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Color).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductNumber).IsRequired();
            });

            modelBuilder.Entity<TblVendor>(entity =>
            {
                entity.HasKey(e => e.VendorId);

                entity.ToTable("tbl_vendor");

                entity.Property(e => e.Name).IsRequired();
            });


            //OnModelCreatingPartial(modelBuilder);

            //Regster store procedure custom object.  
            modelBuilder.Query<SpGetProductByPriceGreaterThan1000>();
            modelBuilder.Query<SpGetProductByID>();
            modelBuilder.Query<SpVendors>();
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #region Get products whose price is greater than equal to 1000 store procedure method.  

        /// <summary>  
        /// Get products whose price is greater than equal to 1000 store procedure method.  
        /// </summary>  
        /// <returns>Returns - List of products whose price is greater than equal to 1000</returns>  
        [Obsolete]
        public async Task<List<SpGetProductByPriceGreaterThan1000>> GetProductByPriceGreaterThan1000Async()
        {
            // Initialization.  
            List<SpGetProductByPriceGreaterThan1000> lst = new List<SpGetProductByPriceGreaterThan1000>();

            try
            {
                // Processing.  
                string sqlQuery = "EXEC [dbo].[GetProductByPriceGreaterThan1000] ";

                lst = await this.Query<SpGetProductByPriceGreaterThan1000>().FromSqlRaw(sqlQuery).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        #endregion

        #region Get product by ID store procedure method.  

        /// <summary>  
        /// Get product by ID store procedure method.  
        /// </summary>  
        /// <param name="productId">Product ID value parameter</param>  
        /// <returns>Returns - List of product by ID</returns>  
        [Obsolete]
        public async Task<List<SpGetProductByID>> GetProductByIDAsync(int productId)
        {
            // Initialization.  
            List<SpGetProductByID> lst = new List<SpGetProductByID>();

            try
            {
                // Settings.  
                SqlParameter usernameParam = new SqlParameter("@product_ID", productId.ToString() ?? (object)DBNull.Value);

                // Processing.  
                //string sqlQuery = "EXEC [dbo].[GetProductByID] " +
                // "@product_ID";

                string sqlQuery = $"EXEC [dbo].[GetProductByID] @product_ID = {productId} ";

                //lst = await this.Query<SpGetProductByID>().FromSqlRaw(sqlQuery, usernameParam).ToListAsync();
                lst = await this.Query<SpGetProductByID>().FromSqlRaw(sqlQuery).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        #endregion

        #region Get Vendors by store procedure method.  

        /// <summary>  
        /// Get Vendors by store procedure method.  
        /// </summary>    
        [Obsolete]
        public async Task<List<SpVendors>> GetVendors()
        {
            // Initialization.  
            List<SpVendors> lst = new List<SpVendors>();

            try
            {        
                string sqlQuery = $"EXEC [dbo].[DisplayVendors]";

                lst = await this.Query<SpVendors>().FromSqlRaw(sqlQuery).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        #endregion

        #region Add new vendors by store procedure method.  

        /// <summary>  
        /// Add new vendors by store procedure method.   
        /// </summary>  
        /// <param name="Name">Name value parameter</param>  
        /// <returns>Returns - add new vendor to the list</returns>  
        [Obsolete]
        public async Task<SpVendors> AddVendors(string Name)
        {
            // Initialization.  
            // List<SpVendors> lst = new List<SpVendors>();
            var vendor = new SpVendors();

            try
            {

                string sqlQuery = $"EXEC AddVendors @Name = \'{Name}\' ";

                var lst = await this.Query<SpVendors>().FromSqlRaw(sqlQuery).ToListAsync();
                //var lst = await this.Query<SpVendors>().FromSqlRaw(sqlQuery).ToListAsync();
                vendor = lst.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return vendor;
        }

        #endregion

        #region Update vendor by store procedure method.  

        /// <summary>  
        /// Update vendors by store procedure method.   
        /// </summary>  
        /// <param name="Name">Name value parameter</param>  
        /// <returns>Returns - update vendor in the list</returns>  
        [Obsolete]
        public async Task<SpVendors> UpdateVendors(string Name, int id)
        {
            // Initialization.  
            SpVendors vendor = new SpVendors();

            try
            {
                string sqlQuery = $"EXEC Sp_UpdateVendors @Name = \'{Name}\', @Id = {id} ";
                var lst = await this.Query<SpVendors>().FromSqlRaw(sqlQuery).ToListAsync();
                vendor = lst.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return vendor;
        }

        #endregion

        #region Delete Vendors using store procedure method.  

        /// <summary>  
        /// Delete Vendors using store procedure method.  
        /// </summary>  
        /// <param name="vendorId">Vendor ID value parameter</param>  
        /// <returns>Returns - Deletes Vendor</returns>  
        [Obsolete]
        public async Task<List<SpVendors>> DeleteVendor(int vendorId)
        {
            // Initialization.  
            List<SpVendors> lst = new List<SpVendors>();

            try
            {

                string sqlQuery = $"EXEC [dbo].[SP_DeleteVendors] @Id = {vendorId} ";

               lst = await this.Query<SpVendors>().FromSqlRaw(sqlQuery).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        #endregion


    }
}
