SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `zera_levi` DEFAULT CHARACTER SET utf8 ;
USE `zera_levi` ;

-- -----------------------------------------------------
-- Table `zera_levi`.`t_people`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_people` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_people` (
  `_id` INT NOT NULL,
  `email` VARCHAR(45) NULL,
  `given_name` VARCHAR(45) NULL,
  `family_name` VARCHAR(45) NULL,
  `address` VARCHAR(300) NULL,
  PRIMARY KEY (`_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_yarthziehts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_yarthziehts` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_yarthziehts` (
  `_id` INT NOT NULL,
  `person_id` INT NOT NULL,
  `relation` VARCHAR(45) NULL,
  `date` DATETIME NOT NULL,
  `deceaseds_name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`_id`),
  INDEX `fk_yarthziehts_people1_idx` (`person_id` ASC),
  CONSTRAINT `fk_yarthziehts_people1`
    FOREIGN KEY (`person_id`)
    REFERENCES `zera_levi`.`t_people` (`_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_accounts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_accounts` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_accounts` (
  `_id` INT NOT NULL,
  `person_id` INT NOT NULL,
  `monthly_total` INT NULL,
  `last_month_paid` DATETIME NULL,
  PRIMARY KEY (`_id`),
  INDEX `fk_accounts_people1_idx` (`person_id` ASC),
  CONSTRAINT `fk_accounts_people1`
    FOREIGN KEY (`person_id`)
    REFERENCES `zera_levi`.`t_people` (`_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_donations`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_donations` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_donations` (
  `_id` INT NOT NULL,
  `account_id` INT NOT NULL,
  `reason` VARCHAR(100) NOT NULL,
  `amount` DOUBLE NOT NULL,
  `date_donated` DATETIME NOT NULL,
  `date_paid` DATETIME NULL,
  `paid` TINYINT(1) NOT NULL,
  `comments` VARCHAR(300) NULL,
  PRIMARY KEY (`_id`),
  INDEX `fk_donations_accounts_idx` (`account_id` ASC),
  CONSTRAINT `fk_donations_accounts`
    FOREIGN KEY (`account_id`)
    REFERENCES `zera_levi`.`t_accounts` (`_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = '	';


-- -----------------------------------------------------
-- Table `zera_levi`.`t_phone_types`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_phone_types` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_phone_types` (
  `_id` INT NOT NULL,
  `type_name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_phone_numbers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_phone_numbers` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_phone_numbers` (
  `person_id` INT NOT NULL,
  `number` VARCHAR(45) NOT NULL,
  `number_type` INT NOT NULL,
  `_id` INT NOT NULL,
  INDEX `fk_phone_numbers_people1_idx` (`person_id` ASC),
  INDEX `fk_phone_numbers_phone_types1_idx` (`number_type` ASC),
  PRIMARY KEY (`_id`),
  CONSTRAINT `fk_phone_numbers_people1`
    FOREIGN KEY (`person_id`)
    REFERENCES `zera_levi`.`t_people` (`_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_phone_numbers_phone_types1`
    FOREIGN KEY (`number_type`)
    REFERENCES `zera_levi`.`t_phone_types` (`_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_privilege_groups`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_privilege_groups` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_privilege_groups` (
  `_id` INT NOT NULL,
  `privileges` VARCHAR(300) NULL,
  PRIMARY KEY (`_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_users`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_users` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_users` (
  `_id` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `privileges_group` INT NOT NULL,
  PRIMARY KEY (`_id`),
  INDEX `fk_users_privilege_groups1_idx` (`privileges_group` ASC),
  CONSTRAINT `fk_users_privilege_groups1`
    FOREIGN KEY (`privileges_group`)
    REFERENCES `zera_levi`.`t_privilege_groups` (`_id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `zera_levi`.`t_privileges`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `zera_levi`.`t_privileges` ;

CREATE TABLE IF NOT EXISTS `zera_levi`.`t_privileges` (
  `_id` INT NOT NULL,
  `privilege_name` VARCHAR(45) NULL,
  PRIMARY KEY (`_id`))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
