<%@ Page Language="C#" AutoEventWireup="false"%>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <link href="layUI/css/layui.css" rel="stylesheet" />
    <link href="layUI/css/layui.mobile.css" rel="stylesheet" />
    <link href="layUI/css/modules/code.css" rel="stylesheet" />
    <link href="layUI/css/modules/layer/default/layer.css" rel="stylesheet" />
    <link href="layUI/css/modules/laydate/default/laydate.css" rel="stylesheet" />
    <script src="Scripts/jquery-3.4.1.js"></script>
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <script src="layUI/layui.all.js"></script>
    <script src="layUI/layui.js"></script>

    <title>演示系统</title>
    <script type="text/javascript">
        function tab1(num) {
            $("#iframeMain").removeAttr("src")
            if (num==1) {
                $("#iframeMain").attr("src", "tab1.html");
            }
            else if (num==2) {
                $("#iframeMain").attr("src", "tab2.html");
            }
           
        }
    function link(url) {
            $("#iframeMain").removeAttr("src");
            $("#iframeMain").attr("src", url);
        }
    </script>
</head>
<body class="layui-layout-body">
    <div class="layui-layout layui-layout-admin">
        <div class="layui-header">
            <div class="layui-logo">演示系统</div>
            <!-- 头部区域（可配合layui已有的水平导航） -->
            <ul class="layui-nav layui-layout-left">
                <li class="layui-nav-item"><a href="javascript:link('/_base/comdic.aspx?name=sys&tag=sys')">字典</a></li>
                <li class="layui-nav-item"><a href="javascript:link('/_base/workflow.aspx')">流程</a></li>
                <li class="layui-nav-item">
                    <a href="javascript:;">其它</a>
                    <dl class="layui-nav-child">
                        <dd><a href="javascript:link('/_pro/admin/user.aspx')">组织架构</a></dd>
                        <dd><a href="javascript:link('/_pro/admin/role.aspx')">角色权限</a></dd>
                        <dd><a href="javascript:link('/_base/apilist.aspx')">Api List</a></dd>
                    </dl>
                </li>
            </ul>
            <ul class="layui-nav layui-layout-right">
                <li class="layui-nav-item">
                    <a href="javascript:;"><%=FTFrame.Tool.UserTool.CurUser().RealName.Equals("")?FTFrame.Tool.UserTool.CurUser().UserName:FTFrame.Tool.UserTool.CurUser().RealName%></a>
                    <dl class="layui-nav-child">
                        <dd><a href="">基本资料</a></dd>
                        <dd><a href="">安全设置</a></dd>
                    </dl>
                </li>
                <li class="layui-nav-item"><a href="/_pro/index.aspx?logout=true">退出</a></li>
            </ul>
        </div>

        <div class="layui-side layui-bg-black">
            <div class="layui-side-scroll">
                <!-- 左侧导航区域（可配合layui已有的垂直导航） -->
                <ul class="layui-nav layui-nav-tree" lay-filter="test">
                    <li class="layui-nav-item layui-nav-itemed">
                        <a class="" href="javascript:;">采购需求</a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:link('/demo/demand.aspx')">我的采购需求</a></dd>
                            <dd><a href="javascript:link('/demo/demandFlow.aspx')">采购需求审批</a></dd>
							<dd><a href="javascript:link('/demo/demandAll.aspx')">所有采购需求</a></dd>
                        </dl>
                    </li>
					<li class="layui-nav-item">
                        <a class="" href="javascript:;">采购订单</a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:link('/demo/po.aspx')">我的采购订单</a></dd>
                            <dd><a href="javascript:link('/demo/poFlow.aspx')">采购订单审批</a></dd>
							<dd><a href="javascript:link('/demo/poAll.aspx')">所有采购订单</a></dd>
                        </dl>
                    </li>
					<li class="layui-nav-item">
                        <a class="" href="javascript:;">采购入库</a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:link('/demo/poIn.aspx')">我的采购入库</a></dd>
							<dd><a href="javascript:link('/demo/poInAll.aspx')">所有采购入库</a></dd>
                        </dl>
                    </li>
                    <li class="layui-nav-item">
                        <a class="" href="javascript:;">报表统计</a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:link('/demo/statPoIn.aspx')">入库明细</a></dd>
							<dd><a href="javascript:link('/demo/statStore.aspx')">库存统计</a></dd>
                        </dl>
                    </li>
                </ul>
            </div>
        </div>

        <div class="layui-body" style='padding-left:4px;background:#ffffff'>
            <!-- 内容主体区域 -->
            <iframe id="iframeMain" width="100%" style="height: 99.6%" frameborder="0"></iframe>
        </div>

        <div class="layui-footer">
            <!-- 底部固定区域 -->
            © layui.com - 底部固定区域
        </div>
    </div>
    <script src="../src/layui.js"></script>
    <script>
        //JavaScript代码区域
        layui.use('element', function () {
            var element = layui.element;

        });
    </script>
</body>
</html>