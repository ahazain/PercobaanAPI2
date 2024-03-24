using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;

namespace PercobaanAPI2.Models
{
    public class PersonContext : IDisposable
    {
        private readonly string _connectionString;

        public PersonContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WebApiDatabase");
        }

        public List<siswa> ListSiswa()
        {
            List<siswa> list1 = new List<siswa>();
            string query = @"SELECT id_siswa, nama, nomor_absen, kelas FROM users.siswa";
            using (var db = new NpgsqlConnection(_connectionString))
            {
                db.Open();
                using (var cmd = new NpgsqlCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list1.Add(new siswa()
                        {
                            id_siswa = reader.GetInt32(0),
                            nama = reader.GetString(1),
                            nomor_absen = reader.GetInt32(2),
                            kelas = reader.GetString(3)
                        });
                    }
                }
            }
            return list1;
        }

        public siswa GetSiswaById(int id)
        {
            string query = @"SELECT id_siswa, nama, nomor_absen, kelas FROM users.siswa WHERE id_siswa = @Id";
            using (var db = new NpgsqlConnection(_connectionString))
            {
                db.Open();
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new siswa()
                            {
                                id_siswa = reader.GetInt32(0),
                                nama = reader.GetString(1),
                                nomor_absen = reader.GetInt32(2),
                                kelas = reader.GetString(3)
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public siswa AddSiswa(siswa newSiswa)
        {
            string query = @"INSERT INTO users.siswa (nama, nomor_absen, kelas) VALUES (@Nama, @NomorAbsen, @Kelas) RETURNING id_siswa";
            using (var db = new NpgsqlConnection(_connectionString))
            {
                db.Open();
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("Nama", newSiswa.nama);
                    cmd.Parameters.AddWithValue("NomorAbsen", newSiswa.nomor_absen);
                    cmd.Parameters.AddWithValue("Kelas", newSiswa.kelas);
                    int newId = (int)cmd.ExecuteScalar();
                    newSiswa.id_siswa = newId;
                }
            }
            return newSiswa;
        }

        public void UpdateSiswa(siswa updatedSiswa)
        {
            string query = @"UPDATE users.siswa SET nama = @Nama, nomor_absen = @NomorAbsen, kelas = @Kelas WHERE id_siswa = @Id";
            using (var db = new NpgsqlConnection(_connectionString))
            {
                db.Open();
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("Nama", updatedSiswa.nama);
                    cmd.Parameters.AddWithValue("NomorAbsen", updatedSiswa.nomor_absen);
                    cmd.Parameters.AddWithValue("Kelas", updatedSiswa.kelas);
                    cmd.Parameters.AddWithValue("Id", updatedSiswa.id_siswa);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSiswa(int id)
        {
            string query = @"DELETE FROM users.siswa WHERE id_siswa = @Id";
            using (var db = new NpgsqlConnection(_connectionString))
            {
                db.Open();
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Dispose()
        {
            // Implementation of IDisposable
        }
    }
}
