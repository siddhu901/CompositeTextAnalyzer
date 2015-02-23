///////////////////////////////////////////////////////////////////////////////
///  MetadataQueries.cs  -  Responsible for handling xml files and display an//
///                         error message with list of files which don't have//
///                         associated xml files(metadata files).            //
///  ver: 1.0                                                                //
///  Language:    C#                                                         //
///  Platform:    Dell Dimension 8100, Windows 2000 Prof., SP2               //
///  Application: CompositeTextAnalyzer, fall 2013                           //
///  Author:      Siddhartha Kakarla, Syracuse University                    //
///               (315) 440-5801, skakarla@syr.edu                           //
///  Description: Retrieves the xml files which contain the metadata         //
///               queries(tags) and print the content of them. Displays the  //
///               files which don't have associated metadata(xml) files      //
///  Functions:   0-arg constructor, processQuery(string[]), xmlFiles(string)//
///               , allXmlFiles(string), checkErrorFiles(), metaDataSearch() //
///  Date:        10/07/2013                                                 //
///////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace CompositeTextAnalyzer
{
  class MetadataQueries
  {

    //instance variables to store the command line arguments and perform other tasks
    CommandLineProcessor cmdlineproc = CommandLineProcessor.objFactory();
    TextQueries textquery = TextQueries.objFactory();
    List<string> metadataqueries=new List<string>();
    List<string> textfiles = new List<string>();
    string path;
    bool recursivefilesearch;
    string[] allfiles;
    List<string> xmlfiles = new List<string>();
    List<string> allxmlfiles = new List<string>();
    SortedList<string,int> validfiles = new SortedList<string,int>();
    List<string> filepatterns;


    //Processes and gets the appropriate command line args using CommandLineProcessor package. Also initiates other tasks 
    public void processQuery(string[] args)
    {
      cmdlineproc.processCmdLine(args);
      path = cmdlineproc.getPath();
      recursivefilesearch = cmdlineproc.getRecursiveFileSearch();
      metadataqueries = cmdlineproc.getMetaDataQueries();
      filepatterns = cmdlineproc.getInpuFilePatterns();
      textfiles = textquery.getAllTextFiles(filepatterns, path);
      xmlFiles(path);
      allXmlFiles(path);
      metaDataSearch();
      checkErrorFiles();
    }//end of processQuery(-)

    //Retrieves all the xml files in the specified path
    public void xmlFiles(string searchpath)
    {
      try
      {
        searchpath = Path.GetFullPath(searchpath);
        Directory.SetCurrentDirectory(searchpath);
        allfiles = Directory.GetFiles(searchpath, "*.xml");
        foreach (string file in allfiles)
        {
          xmlfiles.Add(file);
        }
        //Incase of /R, retrieves the entire xml file set
        if (recursivefilesearch == true)
        {
          string[] dirs = Directory.GetDirectories(searchpath);
          foreach (string dir in dirs)
          {
            //Console.Write("\n {0}",dir);
            xmlFiles(dir);
          }
        }
        //to set the Cuurent Directory to base directory so that exception doesn't occur when allXmlFiles(-) is called
        var appDomain = AppDomain.CurrentDomain;
        string temp = appDomain.BaseDirectory;
        Directory.SetCurrentDirectory(temp);
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
      }
    }//end of xmlFiles(-)

    //To retrieve all xml files in the entire file set irrespective of /R. This will be used in seaching of error files which don't have associated metadata
    public void allXmlFiles(string searchpath)
    {
      try
      {
        searchpath = Path.GetFullPath(searchpath);
        Directory.SetCurrentDirectory(searchpath);
        allfiles = Directory.GetFiles(searchpath, "*.xml");
        foreach (string file in allfiles)
        {
          allxmlfiles.Add(file);
        }
        //Incase of /R, retrieves the entire xml file set
        string[] dirs = Directory.GetDirectories(searchpath);
        foreach (string dir in dirs)
        {
          allXmlFiles(dir);
        }
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
      }
     }//end of allXmlFiles(-)
    

    //Displays all the files which do not have associated metadata files
    void checkErrorFiles()
    {
      try
      {
        int count = 0;
        //read each xml file for the filename and create a list of files which have associated metadata files
        foreach (string xmlfile in allxmlfiles)
        {
          XmlTextReader tr1 = new XmlTextReader(xmlfile);
          while (tr1.Read())
          {
            if (tr1.NodeType == XmlNodeType.Element && tr1.Name.Equals("filename"))
            {
              tr1.Read();
              string filename = tr1.Value;
              filename = Path.GetFileName(filename);
              foreach (string textfile in textfiles)
              {
                string filename2 = Path.GetFileName(textfile);
                if (filename2 == filename)
                {
                  count++;
                  if (count >= 1)
                  {
                    validfiles.Remove(textfile);
                    validfiles.Add(textfile, count);
                  }
                  else
                  {
                    validfiles.Add(textfile, count);
                  }
                  count = 0;
                }

              }
              break;
            }
          }
        }

        Console.Write("\n The files which do not xml links are:-");
        //if all files in file set have no associated metadata file, then print all of them
        if (validfiles.Count == 0)
        {
          foreach (string cursor in textfiles)
          {
            Console.Write("\n {0}", cursor);
          }
        }

        else
        {
          //remove all the valid files from file set
          foreach (KeyValuePair<string, int> cursor in validfiles)
          {
            textfiles.Remove(cursor.Key);
          }
          //Display the left out error files
          foreach (string cursor1 in textfiles)
          {
            Console.Write("\n {0}", cursor1);
          }

        }
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
      }
     }//end of checkErrorFiles()    

    //searches each metadata file and prints the contents of the queries(tags)
    public void metaDataSearch()
    {
      try
      {
        XmlTextReader tr1 = null;
        foreach (string metadaquery in metadataqueries)
        {
          Console.Write("\nThe query is:{0}", metadaquery);
          foreach (string file in xmlfiles)
          {
            tr1 = new XmlTextReader(file);
            while (tr1.Read())
            {
              if (tr1.NodeType == XmlNodeType.Element && tr1.Name.Equals(metadaquery))
              {
                string starttag = tr1.Name;
                Console.Write("\n  Tag:{0}", tr1.Name);
                tr1.Read();
                if (tr1.NodeType == XmlNodeType.Text)
                {
                  Console.Write("\n\tText:{0}", tr1.Value);
                  tr1.Read();
                  Console.Write("\n  EndTag:{0}", tr1.Name);
                }

                else
                {
                  tr1.Read();
                  while (tr1.Name != starttag)
                  {
                    Console.Write("\n\t Tag:{0}", tr1.Name);
                    tr1.Read();
                    Console.Write("\n\t\t Txt: {0}", tr1.Value);
                    tr1.Read();
                    Console.Write("\n\t Tag:{0}", tr1.Name);
                    tr1.Read();
                    tr1.Read();

                  }
                  Console.Write("\n EndTag:{0}", tr1.Name);
                }
              }
            }
          }
          Console.Write("\n");
        }
      }
      catch (Exception exp)
      {
        Console.WriteLine(exp.Message);
      }
     }//end of metadata search


    #if(TEST_CommandLineProcessor)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is MetadataQueries.cs");
              MetadataQueries metadataquery = new MetadataQueries();
              metadataquery.processQuery(args);
              metadataquery.xmlFiles(searchpath);
              metadataquery.allXmlFiles(searchpath);
              metadataquery.checkErrorFiles();
              metadataquery.metaDataSearch();
        }
  
    #endif


  }//end of MetadataQueries class
}//end of CompositeTextAnalyzer namespace
                      
                      
                   
                      
                      
                      

                
