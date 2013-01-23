/*
update masterminute
set Instrument='OLD11ALSI'
where DATEPART(YYYY,Stamp)=2011

update masterminute
set Instrument='MAR12ALSI'
where DATEPART(YYYY,Stamp)=2012 and DATEPART(MM,Stamp)<=3 and DATEPART(DD,Stamp) <15

update masterminute
set Instrument='JUN12ALSI'
where DATEPART(YYYY,Stamp)=2012 and DATEPART(MM,Stamp)=3 and DATEPART(DD,Stamp) >15

update masterminute
set Instrument='JUN12ALSI'
where DATEPART(YYYY,Stamp)=2012 and DATEPART(MM,Stamp)>=4 and DATEPART(MM,Stamp)<=6

update masterminute
set Instrument='SEP12ALSI'
where DATEPART(YYYY,Stamp)=2012 and DATEPART(MM,Stamp)=6 and DATEPART(DD,Stamp)>=21

update masterminute
set Instrument='SEP12ALSI'
where DATEPART(YYYY,Stamp)=2012 and DATEPART(MM,Stamp)>=7 and DATEPART(MM,Stamp)<=9


update masterminute
set Instrument='DEC12ALSI'
where DATEPART(YYYY,Stamp)=2012 and DATEPART(MM,Stamp)=9 and DATEPART(DD,Stamp)>=20

*/
Select * from MasterMinute

where DATEPART(MM,Stamp)>=1 and DATEPART(MM,Stamp)<=4
