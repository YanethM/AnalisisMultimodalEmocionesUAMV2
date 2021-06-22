using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;
using UnityEngine.Networking;

public class BDLocal : MonoBehaviour {

    public UnityEvent iniciarSesion;
    public UnityEvent registrarUsuario;
    public UnityEvent registrarEstudiante;
    public UnityEvent obtenerUsuario;
    public UnityEvent obtenerEstudiantes;
    public UnityEvent estudianteActualizado;
    public UnityEvent estudianteBorrado;

    public UnityEvent registrarProtocolo;
    public UnityEvent protocoloActualizado;
    public UnityEvent protocoloBorrado;
    public UnityEvent obtenerProtocolos;
    public UnityEvent registrarEstudianteProtocolo;
    public UnityEvent actualizarEmotivInfo;
    public UnityEvent obtenerPromedioEmotiv;
    public UnityEvent obtenerEstudianteProtocolo;

    public string rIniciarSesion { get; set; }
    public string rRegistrarUsuario { get; set; }
    public string rRegistrarEstudiante { get; set; }
    public string rObtenerUsuario { get; set; }
    public string rObtenerEstudiantes { get; set; }
    public string rActualizarEstudiante { get; set; }
    public string rBorrarEstudiante { get; set; }
    public string rObtenerPromedioEmotiv { get; set; }

    public string rRegistrarProtocolo { get; set; }
    public string rActualizarProtocolo { get; set; }
    public string rBorrarProtocolo { get; set; }
    public string rObtenerProtocolos { get; set; }
    public string rEstudianteProtocolo { get; set; }
    public string rActualizarEmotivInfo { get; set; }
    public string rObtenerEstudianteProtocolo { get; set; }

    private string urlDBStreamingAsset;
    private string urlDB;
    private string nombreDB;

    private void Start()
    {
        rIniciarSesion = "";
        rRegistrarUsuario = "";
        rRegistrarEstudiante = "";
        rObtenerUsuario = "";
        rObtenerEstudiantes = "";
        rActualizarEstudiante = "";
        rBorrarEstudiante = "";
        rRegistrarProtocolo = "";
        rActualizarProtocolo = "";
        rBorrarProtocolo = "";
        rEstudianteProtocolo = "";
        rObtenerEstudianteProtocolo = "";
        rObtenerProtocolos = "";
        nombreDB = "dbPRE.db";
    }

    private void CopiarBD()
    {      
        if (!Directory.Exists(urlDB))
        {
            try
            {
                Directory.CreateDirectory(urlDB);
            }
            catch (IOException ex)
            {      
                Debug.Log(ex.Message);
            }
#if UNITY_EDITOR
            FileUtil.CopyFileOrDirectory(urlDBStreamingAsset + nombreDB, urlDB + nombreDB);
#endif
        }
    }

    public void IniciarSesion(string email,string contrasena)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            int cantidad = 0;
            string usuContrasena="";

            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Usuario WHERE usu_email = @Email;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Email", Value = email });
                var informacion = comando.ExecuteReader();
               
                while (informacion.Read())
                {
                    usuContrasena = informacion.GetString(4);
                    cantidad++;
                }

                if (cantidad > 0)
                {
                    if (contrasena == usuContrasena)                    
                        rIniciarSesion = "Exito";                    
                    else
                        rIniciarSesion = "Contraseña Erronea";                                 
                } else {
                    rIniciarSesion = "No Existe";
                }
                iniciarSesion.Invoke();
            }
            conexion.Close();
        }
    }

    public void InsertarUsuario(string cedula, string nombre, string apellido, string email, string contrasena, string urlFoto)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            int cantidad = 0;
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Usuario WHERE usu_cedula = @Cedula;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });

                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                    cantidad++;

                if (cantidad == 0)
                {
                    comando.Dispose();
                    comando.CommandText = "INSERT INTO Usuario (usu_cedula, usu_nombre,usu_apellido,usu_email,usu_contrasena,usu_foto) " +
                                      "VALUES (@Cedula, @Nombre, @Apellido, @Email,@Contrasena, @Foto);";
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Nombre", Value = nombre });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Apellido", Value = apellido });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Email", Value = email });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Contrasena", Value = contrasena });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Foto", Value = urlFoto });                    

                    if (comando.ExecuteNonQuery() == 1)                    
                        rRegistrarUsuario = "Exito";                    
                    else
                        rRegistrarUsuario = "Error";                  
                }
                else
                {
                    rRegistrarUsuario = "Existe";
                }
                registrarUsuario.Invoke();
            }
            conexion.Close();
        }
    }

    public void InsertarEstudiante(string cedula, string nombre, string apellido, string email, string departamento, string programa, string sexo, string gafas, string fNacimiento, string semestre, string urlFoto,string habilitado, string urlCuestionario)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            int cantidad = 0;
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Estudiante WHERE est_cedula = @Cedula;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });

                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                    cantidad++;

                if (cantidad == 0)
                {
                    comando.Dispose();
                    comando.CommandText = "INSERT INTO Estudiante (est_cedula, est_nombre,est_apellido,est_email,est_departamento,est_programa,est_sexo,est_gafas,est_nacimiento,est_semestre,est_foto,est_habilitado,est_cuestionario) " +
                                      "VALUES (@Cedula, @Nombre, @Apellido, @Email, @Departamento, @Programa, @Sexo, @Gafas, @FNacimiento, @Semestre, @Foto, @Habilitado, @Cuestionario);";
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Nombre", Value = nombre });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Apellido", Value = apellido });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Email", Value = email });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Departamento", Value = departamento });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Programa", Value = programa });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Sexo", Value = sexo });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Gafas", Value = gafas });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "FNacimiento", Value = fNacimiento });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Semestre", Value = semestre });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Foto", Value = urlFoto });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Habilitado", Value = habilitado });
                    comando.Parameters.Add(new SqliteParameter { ParameterName = "Cuestionario", Value = urlCuestionario });
                    if (comando.ExecuteNonQuery() == 1)                    
                        rRegistrarEstudiante = "Exito";                    
                    else
                        rRegistrarEstudiante = "Error";
                }
                else
                {
                    rRegistrarEstudiante = "Existe";
                }
                registrarEstudiante.Invoke();
            }
            conexion.Close();
        }
    }

    public void ActualizarEstudiante(string cedula, string email, string departamento, string programa, string gafas, string semestre)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "UPDATE Estudiante SET est_email = @Email, est_departamento = @Departamento, est_programa = @Programa, est_gafas = @Gafas," +
                    " est_semestre = @Semestre WHERE est_cedula = @Cedula;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Email", Value = email });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Departamento", Value = departamento });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Programa", Value = programa });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Gafas", Value = gafas });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Semestre", Value = semestre });

                if (comando.ExecuteNonQuery() == 1)
                    rActualizarEstudiante = "Exito";
                else
                    rActualizarEstudiante = "Error";
                estudianteActualizado.Invoke();
            }
            conexion.Close();
        }
    }

    public void BorrarEstudiante(string cedula)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "UPDATE Estudiante SET est_habilitado = 'No' WHERE est_cedula = @Cedula;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });

                if (comando.ExecuteNonQuery() == 1)
                    rBorrarEstudiante = "Exito";
                else
                    rBorrarEstudiante = "Error";
                estudianteBorrado.Invoke();
            }
            conexion.Close();
        }
    }

    public void ObtenerUsuario(string cedula)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Usuario WHERE usu_cedula = @Cedula;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });
                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                {
                    rObtenerUsuario = informacion.GetString(0) + "?" + informacion.GetString(1) + "?" + informacion.GetString(2) + "?" + informacion.GetString(3) + "?" + informacion.GetString(4) + "?" + informacion.GetString(5);
                }
                obtenerUsuario.Invoke();
            }
            conexion.Close();
        }
    }

    public void ObtenerEstudiantes()
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Estudiante WHERE est_habilitado = 'Si';";
                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                {
                    rObtenerEstudiantes += informacion.GetString(0) + "?" + informacion.GetString(1) + "?" + informacion.GetString(2) + "?" 
                        + informacion.GetString(3) + "?" + informacion.GetString(4) + "?" + informacion.GetString(5) + "?" + informacion.GetString(6) + "?"
                        + informacion.GetString(7) + "?" + informacion.GetString(8) + "?" + informacion.GetInt32(9) + "?" + informacion.GetString(10) + "?" 
                        + informacion.GetString(11) + "?" + informacion.GetString(12) + "&";
                }
                obtenerEstudiantes.Invoke();
            }
            conexion.Close();
        }
    }

    public void InsertarProtocolo(string tipo, string nombre, string departamento, string intervalo, string url,string habilitado)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "INSERT INTO Protocolo (prt_tipo, prt_nombre,prt_departamento,prt_intervalo,prt_url,prt_habilitado) " +
                                  "VALUES (@Tipo, @Nombre, @Departamento, @Intervalo, @Url, @Habilitado);";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Tipo", Value = tipo });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Nombre", Value = nombre });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Departamento", Value = departamento });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Intervalo", Value = intervalo });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Url", Value = url });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Habilitado", Value = habilitado });

                if (comando.ExecuteNonQuery() == 1)                
                    rRegistrarProtocolo = "Exito";                
                else
                    rRegistrarProtocolo = "Error";
                registrarProtocolo.Invoke();
            }
            conexion.Close();
        }
    }

    public void ActualizarProtocolo(int id, string intervalo, string url)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "UPDATE Protocolo SET prt_intervalo = @Intervalo, prt_url = @Url WHERE prt_indice = @Id;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Id", Value = id });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Intervalo", Value = intervalo });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Url", Value = url });

                if (comando.ExecuteNonQuery() == 1)
                    rActualizarProtocolo = "Exito";
                else
                    rActualizarProtocolo = "Error";
                protocoloActualizado.Invoke();
            }
            conexion.Close();
        }
    }

    public void BorrarProtocolo(int id)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "UPDATE Protocolo SET prt_habilitado = 'No' WHERE prt_indice = @Id;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Id", Value = id });

                if (comando.ExecuteNonQuery() == 1)
                    rBorrarProtocolo = "Exito";
                else
                    rBorrarProtocolo = "Error";
                protocoloBorrado.Invoke();
            }
            conexion.Close();
        }
    }

    public void ObtenerProtocolos()
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Protocolo WHERE prt_habilitado = 'Si';";
                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                {
                    rObtenerProtocolos += informacion.GetInt32(0) + "?" + informacion.GetString(1) + "?" + informacion.GetString(2) + "?"
                        + informacion.GetString(3) + "?" + informacion.GetString(4) + "?" + informacion.GetString(5) + "?" + informacion.GetString(6) + "&";
                }
                obtenerProtocolos.Invoke();
            }
            conexion.Close();
        }
    }

    public int CantidadRegistros(string tabla)
    {
        int cantidad = 0;
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM " + tabla + ";"; 

                var informacion = comando.ExecuteReader();
                while (informacion.Read())                
                    cantidad++;                
            }
            conexion.Close();
        }
        return cantidad + 1;
    }

    public void InsertarProtocoloEstudiante(string cedula, string nombreProtcolo, string urlArchivo, float alegria, float temor, float disgusto, float tristeza, float enojo, float sorpresa, float desprecio, float valencia, float compromiso)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "INSERT INTO EstudianteProtocolo (est_cedula, prt_nombre,url_archivo,alegria,temor,disgusto,tristeza,enojo,sorpresa,desprecio,valencia,compromiso) " +
                                      "VALUES (@Cedula, @NombreProtocolo, @URL, @Alegria, @Temor, @Disgusto, @Tristeza, @Enojo, @Sorpresa, @Desprecio, @Valencia, @Compromiso);";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "NombreProtocolo", Value = nombreProtcolo });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "URL", Value = urlArchivo });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Alegria", Value = alegria });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Temor", Value = temor });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Disgusto", Value = disgusto });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Tristeza", Value = tristeza });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Enojo", Value = enojo });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Sorpresa", Value = sorpresa });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Desprecio", Value = desprecio });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Valencia", Value = valencia });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Compromiso", Value = compromiso });
                if (comando.ExecuteNonQuery() == 1)
                    rEstudianteProtocolo = "Exito";
                else
                    rEstudianteProtocolo = "Error";

                registrarEstudianteProtocolo.Invoke();
            }
            conexion.Close();
        }
    }

    public void ObtenerProtocolosHechos(string cedula)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM EstudianteProtocolo WHERE est_cedula = @Cedula;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Cedula", Value = cedula });
                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                {
                    rObtenerEstudianteProtocolo += informacion.GetInt32(0) + "?" + informacion.GetString(1) + "?" + informacion.GetString(2) + "?" + informacion.GetString(3) + "?" + informacion.GetFloat(4) + "?" + informacion.GetFloat(5) + "?" + informacion.GetFloat(6) + "?" + informacion.GetFloat(7) + "?" + informacion.GetFloat(8) + "?" + informacion.GetFloat(9) + "?" + informacion.GetFloat(10) + "?" + informacion.GetFloat(11) + "?" + informacion.GetFloat(12) + "?" + informacion.GetInt32(13) + "?" + informacion.GetInt32(14) + "?" + informacion.GetInt32(15) + "?" + informacion.GetInt32(16) + "?" + informacion.GetInt32(17) + "?" + informacion.GetInt32(18) + "&";
                }
                obtenerEstudianteProtocolo.Invoke();
            }
            conexion.Close();
        }
    }

    public void ObtenerPromediosEmotiv(int indice)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM EstudianteProtocolo WHERE indice = @Indice;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Indice", Value = indice });
                var informacion = comando.ExecuteReader();

                while (informacion.Read())
                {
                    rObtenerPromedioEmotiv = informacion.GetInt32(0) + "?" + informacion.GetString(1) + "?" + informacion.GetString(2) + "?" + informacion.GetString(3) + "?" + informacion.GetFloat(4) + "?" + informacion.GetFloat(5) + "?" + informacion.GetFloat(6) + "?" + informacion.GetFloat(7) + "?" + informacion.GetFloat(8) + "?" + informacion.GetFloat(9) + "?" + informacion.GetFloat(10) + "?" + informacion.GetFloat(11) + "?" + informacion.GetFloat(12) + "?" + informacion.GetInt32(13) + "?" + informacion.GetInt32(14) + "?" + informacion.GetInt32(15) + "?" + informacion.GetInt32(16) + "?" + informacion.GetInt32(17) + "?" + informacion.GetInt32(18);
                }
                obtenerPromedioEmotiv.Invoke();
            }
            conexion.Close();
        }
    }

    public void InsertarPromedioEmotiv(int indice, int engagement, int excitement, int stress, int relaxation, int interest, int focus)
    {
        using (var conexion = new SqliteConnection("URI=file:" + nombreDB))
        {
            conexion.Open();
            using (var comando = conexion.CreateCommand())
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = "UPDATE EstudianteProtocolo SET pro_eng = @Engagement, pro_ex = @Excitement, pro_fo = @Focus, pro_in = @Interest, pro_re = @Relaxation, pro_st = @Stress WHERE indice = @Indice;";
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Indice", Value = indice });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Engagement", Value = engagement });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Excitement", Value = excitement });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Stress", Value = stress });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Relaxation", Value = relaxation });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Interest", Value = interest });
                comando.Parameters.Add(new SqliteParameter { ParameterName = "Focus", Value = focus });
                if (comando.ExecuteNonQuery() == 1)
                    rActualizarEmotivInfo = "Exito";
                else
                    rActualizarEmotivInfo = "Error";

                actualizarEmotivInfo.Invoke();
            }
            conexion.Close();
        }
    }
}
