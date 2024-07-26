const toJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    sdt: item.sdt,
    cmnd: item.cmnd,
    ngayKyHopDong: item.ngayKyHopDong,
  };
};

const fromJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    sdt: item.sdt,
    cmnd: item.cmnd,
    ngayKyHopDong: item.ngayKyHopDong,
  };
};

const baseJson = () => {
  return {
    id: null,
    name: null,
    sdt: null,
    cmnd: null,
    ngayKyHopDong: null,
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
