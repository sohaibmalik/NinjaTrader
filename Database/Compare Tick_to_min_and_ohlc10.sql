/****** Script for SelectTopNRows command from SSMS  ******/
select *
  FROM [AlsiTrade].[dbo].[OHLC_10_Minute]
  where DATEPART(dd,stamp)=31 and DATEPART(MM,stamp)=7 and DATEPART(HH,stamp)=11
  
  select *
  FROM [AlsiTrade].[dbo].[MasterMinute]
  where DATEPART(dd,stamp)=31 and DATEPART(MM,stamp)=7 and DATEPART(HH,stamp)=11