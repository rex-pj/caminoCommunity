import React from "react";
import NotFound from "../../components/organisms/Error/NotFound";

export default function (props) {
  return (
    <NotFound
      icon="dizzy"
      title="Có lỗi xảy ra với truy cập của bạn"
      instruction="Liên kết bạn đang truy cập có thể tạm thời bị lỗi, hãy thử lại hoặc quay lại vào lúc khác"
      actionUrl="/"
      actionText="Quay về trang chủ"
    />
  );
}
