<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import { numeric, required } from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import { pagingModel } from "@/models/pagingModel";
import Multiselect from "vue-multiselect";
import { baiThiModel } from "@/models/baiThiModel";

export default {
  page: {
    title: "Bài Thi nè",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: { Layout, PageHeader, Multiselect },
  data() {
    return {
      title: "Thông tin người mua",
      items: [
        {
          text: "Tài khoản",
          href: "/tai-khoan",
        },
        {
          text: "Danh sách",
          active: true,
        },
      ],
      data: [],
      fields: [
        {
          key: "STT",
          label: "STT",
          class: "td-stt",
          sortable: false,
          thClass: "hidden-sortable",
        },
        {
          key: "name",
          label: "Tên người mua",
          class: "td-username",
          sortable: true,
          thStyle: "text-align:center",
          thClass: "hidden-sortable",
        },
        {
          key: "cmnd",
          label: "CMNN/CCCD ",
          class: "td-ten",
          sortable: true,
          thStyle: "text-align:center",
          thClass: "hidden-sortable",
        },
        {
          key: "sdt",
          label: "Số điện thoại ",
          class: "td-ten",
          sortable: true,
          thStyle: "text-align:center",
          thClass: "hidden-sortable",
        },

        {
          key: "process",
          label: "Xử lý",
          class: "td-xuly",
          sortable: false,
          thClass: "hidden-sortable",
        },
      ],
      currentPage: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      filter: null,
      sortDesc: false,
      filterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: baiThiModel.baseJson(),
      listTrangThai: [],
      listLoDat: [],
      itemFilter: {
        content: null,

        trangThaiId: null,
      },
      pagination: pagingModel.baseJson(),
    };
  },
  created() {
    this.getListTrangThai();
    this.getListLoDat();
  },
  methods: {
    async fnGetList() {
      this.$refs.tblList?.refresh();
    },
    async getListTrangThai() {
      await this.$store
        .dispatch("commonStore/getAll", "DM_TRANGTHAI")
        .then((res) => {
          this.listTrangThai = res.data || [];
        });
    },
    async getListLoDat() {
      await this.$store
        .dispatch("commonStore/getAll", "DM_LODAT")
        .then((res) => {
          this.listLoDat = res.data || [];
        });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store
          .dispatch("nguoiMuaStore/delete", { id: this.model.id })
          .then((res) => {
            if (res.code === 0) {
              this.fnGetList();
              this.showDeleteModal = false;
            }
            var a = {
              message: res.message,
              code: res.code,
            };
            //   console.log("LOG A : " ,a)
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;

      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        console.log("LOG USER UPDATE NE : ", this.model);
        // Update model
        await this.$store
          .dispatch("nguoiMuaStore/update", this.model)
          .then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
      } else {
        // Create model
        console.log("LOG USER CREATE NE : ", this.model);
        await this.$store
          .dispatch("nguoiMuaStore/create", this.model)
          .then((res) => {
            if (res.code === 0) {
              this.fnGetList();
              this.showModal = false;
              this.model = {};
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
      }
      loader.hide();
      this.submitted = false;
    },
    async handleUpdate(id) {
      await this.$store
        .dispatch("nguoiMuaStore/getById", { id: id })
        .then((res) => {
          if (res.code === 0) {
            console.log(res);
            this.model = baiThiModel.toJson(res.data);
            this.showModal = true;
          } else {
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          }
        });
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.itemFilter.content,
        trangThaiId: this.itemFilter.trangThaiId,
        startDate: this.itemFilter.ngayBatDau
          ? new Date(this.itemFilter.ngayBatDau)
          : null,
        endDate: this.itemFilter.ngayKetThuc
          ? new Date(this.itemFilter.ngayKetThuc)
          : null,
        sortDesc: ctx.sortDesc,
      };
      this.loading = true;
      try {
        let promise = this.$store.dispatch(
          "nguoiMuaStore/getPagingParams",
          params
        );
        return promise.then((resp) => {
          let items = resp.data.data;
          this.totalRows = resp.data.totalRows;
          this.numberOfElement = resp.data.data.length;
          this.loading = false;
          return items || [];
        });
      } finally {
        this.loading = false;
      }
    },
    addCoQuanToModel(node, instanceId) {
      if (node.id) {
        this.itemFilter.trangThaiId = node.id;
      }
      console.log(" log node ", node);
    },
    handleSearch() {
      console.log("LOG HANDLE SEARCH LICH SU GIAO DICH ", this.itemFilter);
      this.$refs.tblList.refresh();
    },
    handleClear() {
      this.itemFilter = {
        content: null,

        trangThaiId: null,
      };
      this.$refs.tblList.refresh();
    },
  },
  mounted() {},
  watch: {
    model: {
      deep: true,
      handler(val) {
        this.addCoQuanToModel();
        // this.saveValueToLocalStorage()
      },
    },
    showModal(status) {
      if (status == false) this.model = baiThiModel.baseJson();
    },
    // model() {
    //   return this.model;
    // },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="mb-6 col-lg-6">
                <label>Nội dung</label>
                <input
                  v-model="itemFilter.content"
                  type="text"
                  name="untyped-input"
                  class="form-control"
                  placeholder="Tìm kiếm theo tiêu đề"
                  style="height: 39px"
                />
              </div>

              <div class="mb-6 col-lg-6">
                <label>Trạng thái</label>
                <multiselect
                  v-on:select="addCoQuanToModel"
                  :options="listTrangThai"
                  label="name"
                  :multiple="false"
                  :value="itemFilter.trangThaiId"
                  selectLabel="Nhấn vào để chọn lô đất"
                  deselectLabel="Nhấn vào để xóa lô đất"
                  placeholder="Chọn lô đất"
                >
                </multiselect>
              </div>

              <div class="col-sm-12">
                <div class="text-sm-end">
                  <b-button
                    type="button"
                    class="btn-label cs-btn mb-2 me-2"
                    @click="showModal = true"
                    size="sm"
                  >
                    <i class="mdi mdi-plus-box label-icon"></i> Tạo thông tin
                    người mua
                  </b-button>
                  <b-modal
                    v-model="showModal"
                    title="Thông tin người mua"
                    title-class="text-black font-18"
                    body-class="p-3"
                    hide-footer
                    centered
                    no-close-on-backdrop
                    size="lg"
                  >
                    <form @submit.prevent="handleSubmit" ref="formContainer">
                      <div class="row">
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Tên người mua</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id" />
                            <input
                              id="userName"
                              v-model.trim="model.name"
                              type="text"
                              class="form-control"
                              placeholder="Nhập vào tên người mua"
                            />
                          </div>
                        </div>
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Số điện thoại</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id" />
                            <input
                              id="lastName"
                              v-model="model.sdt"
                              type="text"
                              class="form-control"
                              placeholder="Nhập diện tích"
                            />
                          </div>
                        </div>
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">CMN/CCCD</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id" />
                            <input
                              id="lastName"
                              v-model="model.cmnd"
                              type="text"
                              class="form-control"
                              placeholder="Nhập giá trị nền"
                            />
                          </div>
                        </div>
                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button
                          variant="light"
                          @click="showModal = false"
                          class="border-0"
                        >
                          Đóng
                        </b-button>
                        <b-button
                          type="submit"
                          variant="success"
                          class="ms-1 cs-btn"
                          >Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12 text-center">
                <b-button
                  variant=""
                  class="w-10 btn-search"
                  style="
                    margin-right: 10px;
                    height: 40px;
                    width: 130px;
                    font-size: 14px;
                    background-color: #f2b705;
                    border: none;
                    color: #000 !important;
                    font-weight: bold;
                  "
                  size="sm"
                  @click="handleSearch"
                >
                  <i class="bx bx-search font-size-16 align-middle me-2"></i>
                  Tìm kiếm
                </b-button>
                <b-button
                  class="w-10 btn-reset"
                  style="
                    margin-right: 10px;
                    height: 40px;
                    width: 130px;
                    font-size: 14px;
                    background-color: #f2b705;
                    border: none;
                    color: #000 !important;
                    font-weight: bold;
                  "
                  size="sm"
                  @click="handleClear"
                >
                  <i class="mdi mdi-replay font-size-16 align-middle me-2"></i>
                  Làm mới
                </b-button>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div
                      class="col-sm-12 d-flex justify-content-left align-items-center"
                    >
                      <div
                        id="tickets-table_length"
                        class="dataTables_length m-1"
                        style="
                          display: flex;
                          justify-content: left;
                          align-items: center;
                        "
                      >
                        <div class="me-1">Hiển thị</div>
                        <b-form-select
                          class="form-select form-select-sm"
                          v-model="perPage"
                          size="sm"
                          :options="pageOptions"
                          style="width: 70px"
                        ></b-form-select
                        >&nbsp;
                        <div style="width: 100px">dòng</div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                    class="datatables table-admin"
                    :items="myProvider"
                    :fields="fields"
                    striped
                    bordered
                    responsive="sm"
                    :per-page="perPage"
                    :current-page="currentPage"
                    :sort-by.sync="sortBy"
                    :sort-desc.sync="sortDesc"
                    :filter="filter"
                    :filter-included-fields="filterOn"
                    ref="tblList"
                    primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + (currentPage - 1) * perPage + 1 }}
                    </template>
                    <template v-slot:cell(trangThai)="data">
                      <span style="margin-left: 5px">
                        {{ data.item.trangThai.name }}
                      </span>
                    </template>
                    <template v-slot:cell(loDat)="data">
                      <span style="margin-left: 5px">
                        {{ data.item.loDat.name }}
                      </span>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                        type="button"
                        size="sm"
                        class="btn btn-outline btn-sm"
                        v-on:click="handleUpdate(data.item.id)"
                      >
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                        type="button"
                        size="sm"
                        class="btn btn-outline btn-sm"
                        v-on:click="handleShowDeleteModal(data.item.id)"
                      >
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>
                      Hiển thị {{ numberOfElement }} trên tổng số
                      {{ totalRows }} dòng
                    </div>
                  </b-col>
                  <div class="col">
                    <div
                      class="dataTables_paginate paging_simple_numbers float-end"
                    >
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                          v-model="currentPage"
                          :total-rows="totalRows"
                          :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <b-modal
              v-model="showDeleteModal"
              centered
              title="Xóa dữ liệu"
              title-class="font-18"
              no-close-on-backdrop
            >
              <p>Dữ liệu xóa sẽ không được phục hồi!</p>
              <template #modal-footer>
                <button
                  v-b-modal.modal-close_visit
                  class="btn btn-outline-info m-1"
                  v-on:click="showDeleteModal = false"
                >
                  Đóng
                </button>
                <button
                  v-b-modal.modal-close_visit
                  class="btn btn-danger btn m-1"
                  v-on:click="handleDelete"
                >
                  Xóa
                </button>
              </template>
            </b-modal>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-xuly {
  text-align: center;
  width: 20%;
}
</style>
