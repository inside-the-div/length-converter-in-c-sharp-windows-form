CREATE DATABASE length_converter_db
GO
USE length_converter_db
GO
CREATE TABLE length_convert_history (
	converted_id INT IDENTITY(1,1) PRIMARY KEY,
	converted_from VARCHAR(20) NOT NULL,
	converted_to VARCHAR(20) NOT NULL,
	converted_number DECIMAL(18,4) NOT NULL,
	result VARCHAR(50) NOT NULL,
	converted_datetime DATETIME NOT NULL
)