///////////////////////////////////////////////////////////////////////////////
///  TextQueries.cs  -  Responsible for retrieving the files which           //
///                     contain the text queries                             //
///  ver: 1.0                                                                //
///  Language:    C#                                                         //
///  Platform:    Dell Dimension 8100, Windows 2000 Prof., SP2               //
///  Application: CompositeTextAnalyzer, fall 2013                           //
///  Author:      Siddhartha Kakarla, Syracuse University                    //
///               (315) 440-5801, skakarla@syr.edu                           //
///  Description: Searches for the files in the fileset which consist        //
///               the text queires                                           //
///  Functions:   processQuery(string[]), getTextFiles(List<string>,string), //
///               getAllTextFiles(List<string>,string), objFactory(),        //
///               searchText(string)                                         //
///  Date:        10/07/2013                                                 //
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;

namespace CompositeTextAnalyzer
{
  class TextQueries
  {
    //instance variables to store the command line arguments and perform other tasks
    CommandLineProcessor cmdlineproc = CommandLineProcessor.objFactory();
    List<string> fileset = new List<string>();
    List<string> files = new List<string>();
    List<string> filepatterns;
    List<string> textqueries;
    bool recursivefilesearch;
    bool matchalltextqueries;
    string path;
    List<string> matchedfiles = new List<string>();
    string[] allfiles;
    StreamReader reader;

    //uses CommandLineProcessor package to process command line and store corresponding arguments in        instance variables. Then initiates the text serach by calling serachText(string).
    public void processQuery(string[] args)
    {
      cmdlineproc.processCmdLine(args);
      path = cmdlineproc.getPath();
      filepatterns = cmdlineproc.getInpuFilePatterns();
      textqueries = cmdlineproc.getTextQueries();
      recursivefilesearch = cmdlineproc.getRecursiveFileSearch();
      matchalltextqueries = cmdlineproc.getMatchAllQueries();
      searchText(path);
    }

    //Gets all the text files in current directory which match the specified filepatterns
    
   
    public List<string> getTextFiles(List<string> filepatterns, string path)
    {
      try
      {
        List<string> files = new List<string>();
        this.path = Path.GetFullPath(path);
        Directory.SetCurrentDirectory(this.path);
        allfiles = Directory.GetFiles(this.path, "*.*");
        foreach (string iterator in filepatterns)
        {
          foreach (string file in allfiles)
          {
            if (file.EndsWith(iterator))
            {
              files.Add(file);
            }
          }
        }
        return files;
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
        return files;
      } 
    }
    
   
    //Gets all the files in the entire file set(including child directories). Used when /R appears in comamnd line
    public List<string> getAllTextFiles(List<string> filepatterns, string path)
    {
      try
      {
        this.path = Path.GetFullPath(path);
        // Console.Write("\n\n  {0}", path);
        Directory.SetCurrentDirectory(this.path);
        allfiles = Directory.GetFiles(this.path, "*.*");
        foreach (string iterator in filepatterns)
        {

          foreach (string file in allfiles)
          {
            if (file.EndsWith(iterator))
            {
              fileset.Add(file);
            }
          }
        }
        string[] dirs = Directory.GetDirectories(this.path);
        foreach (string dir in dirs)
        {
          getAllTextFiles(filepatterns, dir);
        }
        return fileset;
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
        return files;
      }
    }
        
    //Searches and prints the fully qualified file names which contain the text queries
    public void searchText(string path)
    {

      try
      {
        List<string> matchedfiles = new List<string>();
        //Used as counters if the search is for all matching text queries
        int textqueriescount = textqueries.Count, matchedqueries = 0;
        path = Path.GetFullPath(path);
        Directory.SetCurrentDirectory(path);
        //Gets appropriate files based on /R
        if (recursivefilesearch == true)
          files = getAllTextFiles(filepatterns, path);
        else
          files = getTextFiles(filepatterns, path);

        //text search starts in each file
        foreach (string file in files)
        {
          reader = File.OpenText(file);
          string filecontent = reader.ReadToEnd().ToLower().TrimEnd();
          //If /A is specified in the command line, then searches for all the text queries in the same file
          if (matchalltextqueries == true)
          {
            matchedqueries = 0;
            foreach (string textquery in textqueries)
            {
              string textqry = textquery.ToLower();
              if (filecontent.Contains(textqry))
              {
                matchedqueries++;
                continue;
              }
              else
                break;
            }
            //if file contains all the text queries
            if (matchedqueries == textqueriescount)
            {
              matchedfiles.Add(file);
            }
          }
          //if /O is specified in command line, search only for 1 text query
          else
          {
            foreach (string textquery in textqueries)
            {
              string textqry = textquery.ToLower();
              if (filecontent.Contains(textqry))
              {
                matchedfiles.Add(file);
                break;
              }
            }
          }
        }

        //Printing the matching files     
        if (matchedfiles.Count >= 1)
        {
          Console.Write("\n The files which contain the text quries are:-");
          foreach (string s in matchedfiles)
            Console.Write("\n {0} ", s);
        }
        else
        {
          Console.Write("\n No matching files found");
        }
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
      }
    }//end of searchText(-)

    //factoy method which returns a TextQueries object
    public static TextQueries objFactory()
    {
      return new TextQueries();
    }

    #if(TEST_CommandLineProcessor)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is TextQueries.cs");
              TextQueries textquery = new TextQueries();
              textquery.processQuery(args);
              textquery.searchText(path);
              files=textquery.getTextFiles(filepatterns,path);
              fileset=textquery.getAllTextFiles(filepatterns,path);
              TextQueries textquery1=textquery.objFactory();
        }
  
  #endif
  }//end of TextQueries class
}//end of CompositeTextAnalyzer namespace

