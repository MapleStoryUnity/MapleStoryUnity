﻿/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010, 2015 Snow and haha01haha01
   
 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using System.Collections.Generic;
using System.IO;
using MapleLib.WzLib.Util;
using System;
using System.Drawing;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that can contain sub properties and has one png image
	/// </summary>
    public class WzCanvasProperty : WzExtended, IPropertyContainer
	{
		#region Fields
		internal List<WzImageProperty> properties = new List<WzImageProperty>();
		internal WzPngProperty imageProp;
		internal string name;
		internal WzObject parent;
		//internal WzImage imgParent;
		#endregion

		#region Inherited Members
        public override void SetValue(object value)
        {
            imageProp.SetValue(value);
        }

        public override WzImageProperty DeepClone()
        {
            WzCanvasProperty clone = new WzCanvasProperty(name);
            foreach (WzImageProperty prop in properties)
                clone.AddProperty(prop.DeepClone());
            clone.imageProp = (WzPngProperty)imageProp.DeepClone();
            return clone;
        }

		public override object WzValue { get { return PngProperty; } }
		/// <summary>
		/// The parent of the object
		/// </summary>
		public override WzObject Parent { get { return parent; } internal set { parent = value; } }
		/// <summary>
		/// The WzPropertyType of the property
		/// </summary>
		public override WzPropertyType PropertyType { get { return WzPropertyType.Canvas; } }
		/// <summary>
		/// The properties contained in this property
		/// </summary>
		public override List<WzImageProperty> WzProperties
		{
			get
			{
				return properties;
			}
		}
		/// <summary>
		/// The name of the property
		/// </summary>
		public override string Name { get { return name; } set { name = value; } }
		/// <summary>
		/// Gets a wz property by it's name
		/// </summary>
		/// <param name="name">The name of the property</param>
		/// <returns>The wz property with the specified name</returns>
		public override WzImageProperty this[string name]
		{
			get
			{
				if (name == "PNG")
					return imageProp;
				foreach (WzImageProperty iwp in properties)
					if (iwp.Name.ToLower() == name.ToLower())
                        return iwp;
				return null;
			}
            set
            {
                if (value != null)
                {
                    if (name == "PNG")
                    {
                        imageProp = (WzPngProperty)value;
                        return;
                    }
                    value.Name = name;
                    AddProperty(value);
                }
            }
		}

        public WzImageProperty GetProperty(string name)
        {
            foreach (WzImageProperty iwp in properties)
                if (iwp.Name.ToLower() == name.ToLower())
                    return iwp;
            return null;
        }

		/// Gets a wz property by a path name
		/// </summary>
		/// <param name="path">path to property</param>
		/// <returns>the wz property with the specified name</returns>
		public override WzImageProperty GetFromPath(string path)
		{
			string[] segments = path.Split(new char[1] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (segments[0] == "..")
			{
				return ((WzImageProperty)Parent)[path.Substring(name.IndexOf('/') + 1)];
			}
			WzImageProperty ret = this;
			for (int x = 0; x < segments.Length; x++)
			{
				bool foundChild = false;
				if (segments[x] == "PNG")
				{
					return imageProp;
				}
				foreach (WzImageProperty iwp in ret.WzProperties)
				{
					if (iwp.Name == segments[x])
					{
						ret = iwp;
						foundChild = true;
						break;
					}
				}
				if (!foundChild)
				{
					return null;
				}
			}
			return ret;
		}
		public override void WriteValue(MapleLib.WzLib.Util.WzBinaryWriter writer)
		{
			writer.WriteStringValue("Canvas", 0x73, 0x1B);
			writer.Write((byte)0);
			if (properties.Count > 0)
			{
				writer.Write((byte)1);
				WzImageProperty.WritePropertyList(writer, properties);
			}
			else
			{
				writer.Write((byte)0);
			}
			writer.WriteCompressedInt(PngProperty.Width);
			writer.WriteCompressedInt(PngProperty.Height);
			writer.WriteCompressedInt(PngProperty.format);
			writer.Write((byte)PngProperty.format2);
			writer.Write((Int32)0);
            byte[] bytes = PngProperty.GetCompressedBytes(false);
            writer.Write(bytes.Length + 1);
			writer.Write((byte)0);
            writer.Write(bytes);
		}
		public override void ExportXml(StreamWriter writer, int level)
		{
			writer.WriteLine(XmlUtil.Indentation(level) + XmlUtil.OpenNamedTag("WzCanvas", this.Name, false, false) +
			XmlUtil.Attrib("width", PngProperty.Width.ToString()) +
			XmlUtil.Attrib("height", PngProperty.Height.ToString(), true, false));
			WzImageProperty.DumpPropertyList(writer, level, this.WzProperties);
			writer.WriteLine(XmlUtil.Indentation(level) + XmlUtil.CloseTag("WzCanvas"));
		}
		/// <summary>
		/// Dispose the object
		/// </summary>
		public override void Dispose()
		{
			name = null;
			imageProp.Dispose();
			imageProp = null;
			foreach (WzImageProperty prop in properties)
			{
				prop.Dispose();
			}
			properties.Clear();
			properties = null;
		}
		#endregion

		#region Custom Members
		/// <summary>
		/// The png image for this canvas property
		/// </summary>
		public WzPngProperty PngProperty { get { return imageProp; } set { imageProp = value; } }
		/// <summary>
		/// Creates a blank WzCanvasProperty
		/// </summary>
		public WzCanvasProperty() { }
		/// <summary>
		/// Creates a WzCanvasProperty with the specified name
		/// </summary>
		/// <param name="name">The name of the property</param>
		public WzCanvasProperty(string name)
		{
			this.name = name;
		}
		/// <summary>
		/// Adds a property to the property list of this property
		/// </summary>
		/// <param name="prop">The property to add</param>
		public void AddProperty(WzImageProperty prop)
		{
            prop.Parent = this;
            properties.Add(prop);
		}
		public void AddProperties(List<WzImageProperty> props)
		{
			foreach (WzImageProperty prop in props)
			{
				AddProperty(prop);
			}
		}
		/// <summary>
		/// Remove a property
		/// </summary>
		/// <param name="name">Name of Property</param>
        public void RemoveProperty(WzImageProperty prop)
		{
            prop.Parent = null;
			properties.Remove(prop);
		}

		/// <summary>
		/// Clears the list of properties
		/// </summary>
		public void ClearProperties()
        {
            foreach (WzImageProperty prop in properties) prop.Parent = null;
			properties.Clear();
		}
		#endregion

        #region Cast Values

        public override Bitmap GetBitmap()
        {
            return imageProp.GetPNG(false);
        }
        #endregion
	}
}