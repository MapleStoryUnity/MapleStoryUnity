/**
 * $File: JCS_XML_Loader.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Xml;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// XML loader.
    /// </summary>
    public class JCS_XML_Loader
    {
        private XmlNodeList mNodeList = null;
        private XmlDocument mXmlFile = null;


        public XmlNodeList GetNodeList() { return this.mNodeList; }
        public XmlDocument GetXmlFile() { return this.mXmlFile; }


        public JCS_XML_Loader(TextAsset file)
        {
            mXmlFile = new XmlDocument();
            mXmlFile.LoadXml(file.text);

            mNodeList = mXmlFile.GetElementsByTagName("DialogueContainer");
        }

    }
}
