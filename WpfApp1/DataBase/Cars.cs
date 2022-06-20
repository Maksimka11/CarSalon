//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cars()
        {
            this.CarsInSalon = new HashSet<CarsInSalon>();
        }

        public string GetPhoto
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\Images\" + Photo;
            }
        }

        public string CarName
        {
            get
            {
                return Marks.Name + " " + Model;
            }
        }
    
        public int Car_Id { get; set; }
        public string Photo { get; set; }
        public Nullable<int> Mark { get; set; }
        public string Model { get; set; }
        public Nullable<int> ProductionYear { get; set; }
    
        public virtual Marks Marks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsInSalon> CarsInSalon { get; set; }
    }
}
