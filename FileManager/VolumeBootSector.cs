﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class VolumeBootSector
    {
        ushort BytsPerSector;
	    byte SecPerCluster;
	    ushort SizeOfReserve;//размер разервной области в секторах
	    ushort maxFilesInRoot;//if 0 it's FAT32
	    uint SizeRoot;//размер рут каталога в секторах
	    byte NumOfFATcpy;
	    ushort sizeOfFATcpy;
	    uint totalSec;//всего секторов на диске
	    uint DataSec;//количество скластеров области данных-
	    uint  NumOfClusters;//количество кластеров области данных
	    uint begRoot;//for FAT32


        public VolumeBootSector()
        {
            BytsPerSector = 0;
            SecPerCluster = 0;
            SizeOfReserve = 0;//размер разервной области в секторах
            maxFilesInRoot = 0;//if 0 it's FAT32
            SizeRoot = 0;//размер рут каталога в секторах
            NumOfFATcpy = 0;
            sizeOfFATcpy = 0;
            totalSec = 0;//всего секторов на диске
            DataSec = 0;//количество скластеров области данных-
            NumOfClusters = 0;//количество кластеров области данных
            begRoot = 0;
        }

        public VolumeBootSector(byte[] BPB)
        {
            BytsPerSector = BitConverter.ToUInt16(BPB, 11);;//количество байт на сектор
	        SecPerCluster = BPB[13];//количество секторов на кластер
	        SizeOfReserve = BitConverter.ToUInt16(BPB, 14);//размер разервной области в секторах

	        //---------ROOT_SIZE--------------------------------------------------
	        maxFilesInRoot = BitConverter.ToUInt16(BPB, 17);//if 0 its FAT32?????да, просто размер в 32 изменчив 
            try
            {
                SizeRoot = (maxFilesInRoot * 32U) / BytsPerSector;//размер рут каталога в секторах
            }
            catch(DivideByZeroException)
            {
                SizeRoot = 0;
            }
	        //--------------------------------------------------------------------

	        //------Size of FAT region-----------------------------------
	        NumOfFATcpy = BPB[16];//количество копий фат

	        if (BPB[22] == 0)
		        sizeOfFATcpy =  BitConverter.ToUInt16(BPB, 36);
	        else
		        sizeOfFATcpy = BitConverter.ToUInt16(BPB, 22);
	        //------------------------------------------------------------

	        //--------всего секторов на диске--------------
	        if ( BitConverter.ToUInt16(BPB, 19) != 0 )
		        totalSec = BitConverter.ToUInt16(BPB, 19);
	        else
		        totalSec = BitConverter.ToUInt32(BPB, 32);
	        //----------------------------------------------

	        //Определяем количество секторов области данных
	        DataSec = totalSec - (SizeOfReserve + ((uint)NumOfFATcpy * sizeOfFATcpy) + SizeRoot);

	        //---Определяем количество кластеров области данных-----
            try { 
	            NumOfClusters = DataSec / SecPerCluster;
            }
            catch(DivideByZeroException)
            {
                NumOfClusters = 0;
                //throw new ArgumentException("Data ");
            }

	        begRoot = BitConverter.ToUInt32(BPB, 44);

        }// public VolumeBootSector(byte[] BPB)

        public ushort getBytsPerSector()
        {
            return BytsPerSector;
        }

        public byte getSecPerCluster()
        {
            return SecPerCluster;
        }

        public ushort getSizeOfReserve()
        {
            return  SizeOfReserve;
        }

        public ushort getMaxFilesInRoot()
        {
            return maxFilesInRoot;
        }

        public uint getSizeRoot()
        {
            return SizeRoot;
        }

        public byte getNumOfFatCopy()
        {
            return  NumOfFATcpy;
        }
   
        public uint getTotalSec()
        {
            return totalSec;
        }

        public uint getDataSec()
        {
            return DataSec;
        }

        public uint  getNumOfClusters()
        {
            return NumOfClusters;
        }

        public uint getBegRoot()
        {
            return begRoot;
        }

        public override string ToString()
        {
            return "\nBytes per sector: " + BytsPerSector + "\nSectors per clusters: " + SecPerCluster + "\nSizeOfReserve: " + SizeOfReserve + 
                "\nMaximum files in Root: " + maxFilesInRoot + "\nSizeRoot: " + SizeRoot + "\nNumOfFATcpy: " + NumOfFATcpy +
                 "\nSize of FAT copy: " + sizeOfFATcpy + "\nTotal Secor: " + totalSec + "\nData Sectors" + DataSec + "\nNumOfClusters: " + NumOfClusters
                    + "\nbegRoot: " + begRoot;
        }


    }//~end class

}