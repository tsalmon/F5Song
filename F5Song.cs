using System;
using System.IO;
using TagLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F5Song
{
    class F5SongClass
    {
        static List<TagLib.File> songs;

        public static void help()
        {
            Console.WriteLine("<album>");
            Console.WriteLine("[-begin=X] [-h] [-end=Y] [-track=A] [-text=c|l] [-space=u|s] [-ascii]");
        }

        private static void arguments(string[] args)
        {
            
        }

        static void Main(string[] args)
        {
            Console.WriteLine(composeString(args, ", "));

            if (args.Length < 1)
            {
                help();
            }
            else if(args.Length == 999)
            {
                DirectoryInfo album = null;
                songs = new List<TagLib.File>();
                
                try
                {
                    album = new DirectoryInfo(args[0]);                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                openAlbum(album);
                
                printAlbum();           
            }
       
        }

        private static bool allSameAlbumTitle()
        {
            return true;
        }

        private static bool allSameBand()
        {
            return true;
        }

        private static void openAlbum(DirectoryInfo path_album)
        {
            try //prevent access denied
            {
                foreach (FileInfo file in path_album.GetFiles())
                {                   
                    if(file.Extension != ".mp3"){
                        continue;
                    }
                    songs.Add(TagLib.File.Create(file.FullName));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("access denied");
            }
        }

        private static string composeString(string[] array_string, string sep)
        {
            if(array_string.Length == 0){
                return "";
            }

            string return_string = array_string[0];
            for (int i = 1; i < array_string.Length; i++)
            {
                if(array_string[i] == ""){
                    continue;
                }
                return_string += sep + array_string[i];
            }
            return return_string;
        }

        private static void printAlbum()
        {
           /* if (songs == null)
            {
                Console.WriteLine("empty");
                return;
            }*/
            Console.WriteLine("Id\tSong\tBand\ttime");


            foreach(TagLib.File song in songs)
            {
                Console.WriteLine(songs.Count);
                string[] infos = new string[11];
                infos[0] = song.Tag.Copyright;
                infos[1] = song.Tag.Album;

                infos[2] = composeString(song.Tag.AlbumArtists, ";");

                infos[3] = song.Tag.Title;
                infos[4] = song.Tag.Comment;

                infos[5] = composeString(song.Tag.Composers, ";");

                infos[6] = song.Tag.Conductor;
                infos[7] = song.Tag.Grouping;

                infos[8] = composeString(song.Tag.Performers, ";");
                infos[9] = composeString(song.Tag.Genres, ";");
                infos[10] = ""+song.Tag.Track;


                Console.WriteLine(composeString(infos, "\t"));

            }
            
        }
    }
}