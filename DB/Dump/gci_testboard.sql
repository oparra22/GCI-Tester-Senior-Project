CREATE DATABASE  IF NOT EXISTS `gci` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `gci`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: localhost    Database: gci
-- ------------------------------------------------------
-- Server version	5.5.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `testboard`
--

DROP TABLE IF EXISTS `testboard`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `testboard` (
  `TestBoardEntryID` int(11) NOT NULL AUTO_INCREMENT,
  `TestBoardID` int(11) NOT NULL,
  `BoardName` varchar(45) NOT NULL,
  `PartID` int(11) NOT NULL,
  `SocketIndex` int(11) NOT NULL,
  `SocketName` varchar(45) NOT NULL,
  `DUTPin` int(11) NOT NULL,
  `GCITesterPIN` int(11) NOT NULL,
  `CreationDate` datetime NOT NULL,
  `LastEditDate` datetime NOT NULL,
  PRIMARY KEY (`TestBoardEntryID`),
  UNIQUE KEY `PinMapEntryID_UNIQUE` (`TestBoardEntryID`),
  KEY `TestBoardID` (`TestBoardID`),
  KEY `BoardName` (`BoardName`),
  KEY `PartID` (`PartID`),
  KEY `SocketName` (`SocketName`)
) ENGINE=InnoDB AUTO_INCREMENT=152 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-10-31 13:29:57
