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
-- Table structure for table `lifetimedata`
--

DROP TABLE IF EXISTS `lifetimedata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `lifetimedata` (
  `LifetimeEntryID` int(11) NOT NULL AUTO_INCREMENT,
  `LifetimeTestID` int(11) DEFAULT NULL,
  `SerialNumber` varchar(50) NOT NULL,
  `BatchName` varchar(50) NOT NULL,
  `PartID` int(11) NOT NULL,
  `TestHour` int(11) NOT NULL,
  `LifetimeLimitID` int(11) NOT NULL,
  `Temperature` double NOT NULL,
  `DUTPinNumber` int(11) NOT NULL,
  `MeasurementNumber` int(11) NOT NULL,
  `MeasuredVoltage` double NOT NULL,
  `AverageVoltage` double NOT NULL,
  `StdDevVoltage` double NOT NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`LifetimeEntryID`),
  UNIQUE KEY `LifetimeEntryID_UNIQUE` (`LifetimeEntryID`),
  KEY `SerialNumber` (`SerialNumber`),
  KEY `BatchNumber` (`BatchName`),
  KEY `TestHour` (`TestHour`),
  KEY `PinNumber` (`DUTPinNumber`),
  KEY `DutPinNumber` (`DUTPinNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=1265 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-10-31 13:30:03
