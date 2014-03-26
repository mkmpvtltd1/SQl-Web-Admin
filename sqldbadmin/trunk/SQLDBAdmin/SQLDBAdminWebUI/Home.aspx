<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DB Admin</title>
</head>
<frameset id="top_frameset" rows="70,*" frameborder="1" bordercolor="#F4F4F4">
		<frame name="top" id="top" frameborder="1" noresize="noresize" scrolling="no" src="Top.aspx"/>
<frameset id="main_frameset" cols="350,*" frameborder="1" bordercolor="#F4F4F4" >
		<frame name="nav" id="nav" frameborder="1"  scrolling="auto"  src="DatabaseList.aspx" />		
		<frame name="content" id="content" frameborder="1" scrolling="auto" src="Query.aspx"/>
		<noframes>
			<body>
				<script language="javascript" type="text/javascript">
				<!--
					alert("In order to use DBAdmin in the best conditions, please use a browser which accepts frames");
				//-->
				</script>
			</body>
		</noframes>
</frameset>
	</frameset>
</html>
