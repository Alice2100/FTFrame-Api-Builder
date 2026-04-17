using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTFrame.Project.Core;
using FTFrame.Project.Core.Utils;
using FTFrame.DBClient;
using FTFrame.Tool;
namespace FTFrame.WebApi
{
    [Route("api/File/[action]")]
    [ApiController]
    public class FileManage : ControllerBase
    {
        [HttpPost]
        public IActionResult Upload([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            /*
  CREATE TABLE `sys_file_info` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT COMMENT '表唯一标识',
  `file_code` varchar(36) NOT NULL DEFAULT '' COMMENT '文件编码',
  `file_serial_numbers` varchar(36) NOT NULL DEFAULT '' COMMENT '文件业务流水号',
  `file_name` varchar(256) NOT NULL DEFAULT '' COMMENT '文件名称',
  `file_original_name` varchar(128) NOT NULL DEFAULT '' COMMENT '文件原名称',
  `file_type` varchar(12) NOT NULL DEFAULT '' COMMENT '文件类型',
  `file_size` varchar(32) NOT NULL DEFAULT '' COMMENT '文件大小',
  `file_url` varchar(256) NOT NULL DEFAULT '' COMMENT '文件地址',
  `file_path` varchar(128) NOT NULL DEFAULT '' COMMENT '文件位置',
  `file_click_num` bigint(20) NOT NULL DEFAULT '0' COMMENT '视频播放数/文件下载数',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_by` varchar(36) NOT NULL DEFAULT 'root' COMMENT '创建操作员',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `update_by` varchar(36) NOT NULL DEFAULT 'root' COMMENT '更新操作员',
  `remark` varchar(256) NOT NULL DEFAULT '' COMMENT '备注',
  `is_delete_flag` tinyint(1) NOT NULL DEFAULT '1' COMMENT '是否已删除 0:删除；1:生效',
  `version` int(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1096 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='文件记录表';
             */
            return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
        }
    }
}
