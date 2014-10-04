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
        private static List<TagLib.File> songs;
        private static bool arg_text;
        private static string arg_begin;
        private static string arg_end;
        private static int arg_incTrack;
        private static string[] arg_involvedartist;
        private static string arg_albumartist;
        private static string arg_year;
        private static string[] arg_genre;
        private static string arg_album_name;
        private static string arg_album_path;

        public static void help()
        {
            Console.WriteLine("F5Song - un programme pour editer les mp3");

            Console.WriteLine("Argument obligatoire");
            Console.WriteLine("<path album>");

            Console.WriteLine("Arguments de selection");
            Console.WriteLine("[-begin=X] debut des numeros de mp3 selectionées");
            Console.WriteLine("[-end=Y] fin des numeros de mp3 selectionnées");

            Console.WriteLine("Arguments optionels");
            Console.WriteLine("[-track=A] [-text] [-involved=] [-albumartist=] [-year=] [-genre=]");
        }

        private static string[] stringToArray(string str, char sep)
        {
            string[] words = str.Split(sep);
            return words;
        }

        private static bool setArguments(string arg_var)
        {
            string[] words = arg_var.Split('=');
            if (words.Length != 2)
            {
                Console.WriteLine("Argument invalide : " + arg_var);
                return false;
            }
            switch (words[0])
            {
                case "-b":
                case "-begin":
                    arg_begin = words[0];
                    break;
                case "-e":
                case "-end":
                    arg_end = words[1];
                    break;
                case "-t":
                case "-track":
                    arg_incTrack = Convert.ToInt32(words[1]);
                    break;
                case "-txt":
                case "-text":
                    arg_text = true;
                    break;
                case "-a":
                case "-albumartist":
                    arg_albumartist = words[1];
                    break;
                case "-n":
                case "-albumname":
                    arg_album_name = words[1];
                    break;
                case "-i":
                case "-involved":
                    arg_involvedartist = stringToArray(words[1], ';');
                    break;
                case "-y":
                case "-year":
                    arg_year = words[1];
                    break;
                case "-g":
                case "-genre":
                    arg_genre = stringToArray(words[1], ';');
                    break;
                default:
                    Console.WriteLine("Argument inexistante: " + arg_var);
                    return false;
            }
            return true;
        }

        private static bool parseArguments(string[] args)
        {
            arg_album_path = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                if (!setArguments(args[i]))
                {
                    Console.WriteLine("Cet argument n'existe pas");
                    return false;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                help();
            }
            else
            {
                parseArguments(args);

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
                    if (file.Extension != ".mp3")
                    {
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
            if (array_string.Length == 0)
            {
                return "";
            }

            string return_string = array_string[0];
            for (int i = 1; i < array_string.Length; i++)
            {
                if (array_string[i] == "")
                {
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


            foreach (TagLib.File song in songs)
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
                infos[10] = "" + song.Tag.Track;


                Console.WriteLine(composeString(infos, "\t"));

            }

        }
    }
}