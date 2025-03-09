namespace contact_management;

public class User
{
    public int c_id { get; set; }
    public string c_name { get; set; }
    public string c_email { get; set; }
    public string c_password { get; set; }
    public string? c_image { get; set; }
    public string c_gender { get; set; }         
    public string c_address { get; set; }         
    public string c_mobile { get; set; }
    public IFormFile? ProfileImage { get; set; }
    
}

    

