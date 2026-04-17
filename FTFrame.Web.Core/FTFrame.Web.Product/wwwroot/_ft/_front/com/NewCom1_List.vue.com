/*View code reference:
1、<NewCom1_List ref="NewCom1_List"/>
2、import NewCom1_List from '@/ftdp/components/NewCom1_List?'
3、components: { NewCom1_List }
*/
/*List component methods, properties, events:
Event callback:
	@beforeLoad: The event before obtaining data from the interface, with optional parameters being the component object
	@beforeSet: an event that has not yet been assigned a value after obtaining data from the interface, with optional parameters being the component object
	@afterLoad: Event after assignment, optional parameter is component object
Retention methods and attributes:
	load(paras): Load, optional parameter is the sequence parameter of apiUrl
	search(): Search
	reset(): Reset
	excel(maxRow,exportName): Export to Excel
	selection(): Return selected object
	fileDownload(fileInfo): File Download
	get(apiPath, paras, callback): Get call
	post(apiPath, paras, json, callback): Post call
	batch(keyName, updateName, updateVal): Batch operations
	loadauto: The default attribute value is' 1 ', will it be automatically loaded
*/
<template>
	<el-container>
		<el-header>
			<el-input clearable v-model="sdata.name" style="width:200px;margin-right:10px"></el-input>
			<el-select clearable v-model="sdata.sex.selValue" style="width:200px;margin-right:10px">
				<el-option v-for="item in sdata.sex.options" :key="item.value" :label="item.label" :value="item.value"></el-option>
			</el-select>
			<el-button type="primary" icon="el-icon-success" @click="search()">Search</el-button>
			<el-button type="success" icon="el-icon-plus" @click="add()">Add</el-button>
			<el-button type="info" icon="el-icon-eleme" @click="batch(keyName, updateName, updateVal, paras)">Batch</el-button>
		</el-header>
		<el-main>
			<el-table :data="gridData.list" @selection-change="handleSelectionChange" @sort-change="handleSortChange">
				<el-table-column label="Primary Key Value" property="fid"></el-table-column>
				<el-table-column label="Primary Key" property="id"></el-table-column>
				<el-table-column label="Name" property="name"></el-table-column>
				<el-table-column label="Phone Number" property="phone"></el-table-column>
				<el-table-column label="Sex" property="sex"></el-table-column>
				<el-table-column label="Education Background" property="education"></el-table-column>
				<el-table-column label="notes" property="mimo"></el-table-column>
				<el-table-column label="State" property="stat"></el-table-column>
				<el-table-column label="Add Time" property="_add_time"></el-table-column>
				<el-table-column label="Update Time" property="_update_time"></el-table-column>
				<el-table-column label="Add by UserId" property="_add_user"></el-table-column>
				<el-table-column label="Update by UserId" property="_update_user"></el-table-column>
				<el-table-column label="Available Flag" property="_available_flag"></el-table-column>
			</el-table>
		</el-main>
		<el-footer>
			<el-pagination layout="prev, pager, next" @size-change="handleSizeChange"  @current-change="handleCurrentChange" :total="page.total" :page-size="page.size" />
		</el-footer>
	</el-container>
</template>

<script>
import { ftdpConfig } from '@/ftdp/utils/config.js'
import { ftdpBase } from '@/ftdp/utils/base.js'
//Config
var apiBase=ftdpConfig.apiBase
var config={
	apiUrl:apiBase+"/new_page_1?list",
	apiPara: '',
	tokenKey:ftdpConfig.tokenKey,
	tokenVal:ftdpConfig.getTokenLocal(),
	orderBy:'',
	orderType:'',
	schText:'',
	schStrict:'',
	pageSize:12,
	pageNum:1,
}
//Search Data Definition
var sdata={
	name:"",
	sex:{
		options:[{value:'1',label:'f'},{value:'2',label:'m'}],
		selValue:''
	},
}

var orisdata = '{}'
var inited = false
export default  {
    data: function() {
        return {
          gridData:{list: [] },
          multipleSelection: [],
          page:{count: 0, size: config.pageSize,loading:false },
        ftdpConfig: ftdpConfig,
            fillPara:'',
          sdata:sdata
        }
    },
	props: {
    loadauto: {
      type: String,
      default: '1'
    }
  },
	methods: {
		load(paras) {
            if (paras != null)config.apiPara = paras
			list_data_module(this);
		},
		search() {
			search_build(this);
			list_data_module(this);
		},
		reset() {
			ftdpBase.list_reset(sdata, orisdata);
			reset_build(this);
			list_data_module(this);
		},
        func(f) {
        const Fn = Function
        new Fn(f + '(this)')()
        },
		excel(maxRow,exportName){
			ftdpBase.list_excel(config,this,maxRow,exportName);
		},
        selection(){
			return this.multipleSelection;
        },
		handleSelectionChange(val) {
			this.multipleSelection = val;
       },
		handleSortChange(val) {
			if(val.order==null)
			{
				config.orderBy='';
				config.orderType='';
			}else if(val.order==='ascending')
			{
				config.orderBy=val.prop;
				config.orderType='asc';
			}
			else {
				config.orderBy=val.prop;
				config.orderType='desc';
			}
			list_data_module(this);
       },
		handleCurrentChange(val) {
         config.pageNum=val;
		list_data_module(this);
      },
    handleSizeChange(val) {
    config.pageSize=val;
    list_data_module(this);
    },
get(apiPath, paras, callback) {
      ftdpBase.normal_get(this, ftdpConfig, apiPath, paras, callback)
    },
    post(apiPath, paras, json, callback) {
      ftdpBase.normal_post(this, ftdpConfig, apiPath, paras, json, callback)
    },
batch(keyName, updateName, updateVal,paras) {
      var keyValues = ''
      for (let i = 0; i < this.selection().length; i++) {
        if (i > 0)keyValues += ','
        keyValues += this.selection()[i][keyName]
      }
      if (keyValues === '') {
        this.$message.warning('Please Select Item')
      } else {
        this.post(config.apiUrl,paras, JSON.parse('{"keyValue":"' + keyValues + '","' + updateName + '":"' + updateVal + '"}'), () => { list_data_module(this) })
      }
    },
fileDownload(fileInfo) {
    ftdpBase.file_download(this, ftdpConfig, fileInfo)
    }
   },
	mounted:function(){
        if (!inited) {
		init(this);
		orisdata = JSON.stringify(sdata);}
		if(this.loadauto==='1')list_data_module(this);
        inited = true
	}
}
//Component internal encapsulation loading method
function list_data_module(vm)
{
	ftdpBase.list_data(config,vm,()=>{return js_beforeload(vm)},(resData)=>{return js_beforeset(vm,resData)},(resData)=>{js_afterset(vm,resData)});
}
//Initialize data definitions, such as data from dropdown search boxes
function init(vm)
{
config.orderBy="";
config.orderType="";
config.schText="";
config.schStrict="";
config.pageSize=12;
config.pageNum=1;
}
//Query parameter construction
function search_build(vm)
{
config.schText="";
config.schStrict=""
		+ ";name:%" + sdata.name + "%" 
		+ ";sex:" + sdata.sex.selValue
;
}
//Build upon reset
function reset_build(vm)
{

}
//Load pre script, return false to block
function js_beforeload(vm)
{
vm.$emit('beforeLoad', vm);
return true;
}
//Script before assignment, return false to block
function js_beforeset(vm,resData)
{
//resData is json object return from Api
vm.$emit('beforeSet', vm);
return true;
}
//Script after assignment
function js_afterset(vm,resData)
{
vm.$emit('afterLoad', vm);
//resData is json object return from Api
}
</script>
<style scoped>
.el-header, .el-footer {
	text-align: right;
  }
</style>
