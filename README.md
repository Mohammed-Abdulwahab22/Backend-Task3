# Backend-Task3

Backend task3:
Add the following operations to your API 
1. Filter by creation data:
	Form fields:
	StartDate ISO formatted date string
	[Optional] EndDate ISO formatted date string
	[Optional] SortType : Enumeration, 
Sort by CreationDate in ascending/descending order, 
sort by ModificationDate in ascending/descending order

	Find all the files with a creation date that is larger than the given StartDate and if present, Smaller than the end date and return their names as a json array of objects that contain(
	The file's name, Creation and Modification dates.

 If no sort type is provided sort them by creation date in ascending order
	If one of the required fields is missing the API should return a bad request response code


2. Filter by user
	Form fields:
	UserName string[] 
	[Optional] StartDate ISO formatted date string 
	[Optional] EndDate ISO formatted date string
	[Optional] SortType : Enumeration, Sort by CreationDate in ascending/descending order, sort by ModificationDate in ascending/descending order
	Find all files that belong to each of the given usernames and return each record(not just file's name) as a json array of objects alongside their Creation and Modification dates 
	if the StartDate, EndDate or both are present than only return the records with creation dates that comply with dates
	if no sort type is provided sort them by creation date in ascending order
	If one of the required fields is missing the API should return a bad request response code
