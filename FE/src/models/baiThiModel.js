import moment from "moment";
const toJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    dienTich: item.dienTich,
    giaTriNen: item.giaTriNen,
    trangThai: item.trangThai,
    diaChi: item.diaChi,
    ghiChu: item.ghiChu,

    tenNguoiMua: item.tenNguoiMua,
    sdt: item.sdt,
    cmnd: item.cmnd,
    ngayKyHopDong: item.ngayKyHopDong,
    ngayKyHopDongShow: item.ngayKyHopDongShow,
  };
};

const fromJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    dienTich: item.dienTich,
    giaTriNen: item.giaTriNen,
    trangThai: item.trangThai,
    diaChi: item.diaChi,
    ghiChu: item.ghiChu,

    tenNguoiMua: item.tenNguoiMua,
    sdt: item.sdt,
    cmnd: item.cmnd,
    ngayKyHopDong: item.ngayKyHopDong,
    ngayKyHopDongShow: item.ngayKyHopDongShow,
  };
};

const baseJson = () => {
  return {
    id: null,
    name: null,
    dienTich: 0,
    giaTriNen: 0,
    trangThai: null,
    diaChi: null,
    ghiChu: null,

    tenNguoiMua: null,
    sdt: null,
    cmnd: null,
    ngayKyHopDong: null,
    ngayKyHopDongShow: null,
  };
};

const toListModel = (items) => {
  if (items.length > 0) {
    let data = [];
    items.map((value, index) => {
      data.push(fromJson(value));
    });
    return data ?? [];
  }
  return [];
};
export const baiThiModel = {
  toJson,
  fromJson,
  baseJson,
  toListModel,
};
