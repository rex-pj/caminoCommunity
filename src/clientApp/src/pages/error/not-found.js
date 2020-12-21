import React from "react";
import NotFound from "../../components/organisms/Error/NotFound";

export default function (props) {
  return (
    <NotFound
      icon="unlink"
      title="Không tìm thấy trang này"
      instruction="Liên kết bạn đang truy cập có thể bị gỡ hoặc không tồn tại"
      actionUrl="/"
      actionText="Quay về trang chủ"
    />
  );
}
