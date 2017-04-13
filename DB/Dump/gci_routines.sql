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
-- Temporary table structure for view `productiondata_v`
--

DROP TABLE IF EXISTS `productiondata_v`;
/*!50001 DROP VIEW IF EXISTS `productiondata_v`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `productiondata_v` (
  `ProductionTestID` int(11),
  `CreationDate` datetime,
  `BatchName` varchar(45),
  `PartName` varchar(45),
  `DutPinNumber` int(11),
  `BaselineVoltage` double,
  `AverageVoltage` double,
  `StdDevVoltage` double,
  `TestResult` bit(1)
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `lifetimedata_v_individuals`
--

DROP TABLE IF EXISTS `lifetimedata_v_individuals`;
/*!50001 DROP VIEW IF EXISTS `lifetimedata_v_individuals`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `lifetimedata_v_individuals` (
  `LifetimeTestID` int(11),
  `SerialNumber` varchar(50),
  `BatchName` varchar(50),
  `PartName` varchar(45),
  `TestHour` int(11),
  `LowerRange` double,
  `UpperRange` double,
  `Temperature` double,
  `DUTPinNumber` int(11),
  `MeasurementNumber` int(11),
  `MeasuredVoltage` double,
  `CreationDate` datetime
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `lifetimedata_v_average`
--

DROP TABLE IF EXISTS `lifetimedata_v_average`;
/*!50001 DROP VIEW IF EXISTS `lifetimedata_v_average`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `lifetimedata_v_average` (
  `LifetimeTestID` int(11),
  `SerialNumber` varchar(50),
  `BatchName` varchar(50),
  `PartName` varchar(45),
  `TestHour` int(11),
  `LowerRange` double,
  `UpperRange` double,
  `Temperature` double,
  `DUTPinNumber` int(11),
  `AverageVoltage` double,
  `StdDevVoltage` double,
  `CreationDate` datetime
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `productiondata_v`
--

DROP TABLE IF EXISTS `productiondata_v`;
DROP VIEW IF EXISTS `productiondata_v`;
SET @saved_cs_client          = @@character_set_client;
SET @saved_cs_results         = @@character_set_results;
SET @saved_col_connection     = @@collation_connection;
SET character_set_client      = utf8;
SET character_set_results     = utf8;
SET collation_connection      = utf8_general_ci;
CREATE ALGORITHM=UNDEFINED 
DEFINER=`root`@`localhost` SQL SECURITY DEFINER
VIEW `productiondata_v` AS select `pd`.`ProductionTestID` AS `ProductionTestID`,`pd`.`CreationDate` AS `CreationDate`,`pd`.`BatchName` AS `BatchName`,`p`.`PartName` AS `PartName`,`pd`.`DUTPinNumber` AS `DutPinNumber`,`pl`.`AverageVoltage` AS `BaselineVoltage`,`pd`.`AverageVoltage` AS `AverageVoltage`,`pd`.`StdDevVoltage` AS `StdDevVoltage`,`pd`.`TestResult` AS `TestResult` from ((`productiondata` `pd` left join `part` `p` on((`pd`.`PartID` = `p`.`PartID`))) left join `productionlimits` `pl` on(((`pd`.`ProductionLimitID` = `pl`.`ProductionLimitID`) and (`pd`.`DUTPinNumber` = `pl`.`DUTPinNumber`)))) where ((`p`.`IsActive` = 1) and (`pd`.`MeasurementNumber` = 0));
SET character_set_client      = @saved_cs_client;
SET character_set_results     = @saved_cs_results;
SET collation_connection      = @saved_col_connection;

--
-- Final view structure for view `lifetimedata_v_individuals`
--

DROP TABLE IF EXISTS `lifetimedata_v_individuals`;
DROP VIEW IF EXISTS `lifetimedata_v_individuals`;
SET @saved_cs_client          = @@character_set_client;
SET @saved_cs_results         = @@character_set_results;
SET @saved_col_connection     = @@collation_connection;
SET character_set_client      = utf8;
SET character_set_results     = utf8;
SET collation_connection      = utf8_general_ci;
CREATE ALGORITHM=UNDEFINED
DEFINER=`root`@`localhost` SQL SECURITY DEFINER
VIEW `lifetimedata_v_individuals` AS select `ld`.`LifetimeTestID` AS `LifetimeTestID`,`ld`.`SerialNumber` AS `SerialNumber`,`ld`.`BatchName` AS `BatchName`,`p`.`PartName` AS `PartName`,`ld`.`TestHour` AS `TestHour`,`ll`.`LowerRange` AS `LowerRange`,`ll`.`UpperRange` AS `UpperRange`,`ld`.`Temperature` AS `Temperature`,`ld`.`DUTPinNumber` AS `DUTPinNumber`,`ld`.`MeasurementNumber` AS `MeasurementNumber`,`ld`.`MeasuredVoltage` AS `MeasuredVoltage`,`ld`.`CreationDate` AS `CreationDate` from ((`lifetimedata` `ld` left join `part` `p` on((`ld`.`PartID` = `p`.`PartID`))) left join `lifetimelimit` `ll` on((`ld`.`LifetimeLimitID` = `ll`.`LifetimeLimitID`))) where (`p`.`IsActive` = 1);
SET character_set_client      = @saved_cs_client;
SET character_set_results     = @saved_cs_results;
SET collation_connection      = @saved_col_connection;

--
-- Final view structure for view `lifetimedata_v_average`
--

DROP TABLE IF EXISTS `lifetimedata_v_average`;
DROP VIEW IF EXISTS `lifetimedata_v_average`;
SET @saved_cs_client          = @@character_set_client;
SET @saved_cs_results         = @@character_set_results;
SET @saved_col_connection     = @@collation_connection;
SET character_set_client      = utf8;
SET character_set_results     = utf8;
SET collation_connection      = utf8_general_ci;
CREATE ALGORITHM=UNDEFINED 
DEFINER=`root`@`localhost` SQL SECURITY DEFINER
VIEW `lifetimedata_v_average` AS select `ld`.`LifetimeTestID` AS `LifetimeTestID`,`ld`.`SerialNumber` AS `SerialNumber`,`ld`.`BatchName` AS `BatchName`,`p`.`PartName` AS `PartName`,`ld`.`TestHour` AS `TestHour`,`ll`.`LowerRange` AS `LowerRange`,`ll`.`UpperRange` AS `UpperRange`,`ld`.`Temperature` AS `Temperature`,`ld`.`DUTPinNumber` AS `DUTPinNumber`,`ld`.`AverageVoltage` AS `AverageVoltage`,`ld`.`StdDevVoltage` AS `StdDevVoltage`,`ld`.`CreationDate` AS `CreationDate` from ((`lifetimedata` `ld` left join `part` `p` on((`ld`.`PartID` = `p`.`PartID`))) left join `lifetimelimit` `ll` on((`ld`.`LifetimeLimitID` = `ll`.`LifetimeLimitID`))) where ((`ld`.`MeasurementNumber` = 0) and (`p`.`IsActive` = 1));
SET character_set_client      = @saved_cs_client;
SET character_set_results     = @saved_cs_results;
SET collation_connection      = @saved_col_connection;

--
-- Dumping routines for database 'gci'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-10-31 13:30:04
