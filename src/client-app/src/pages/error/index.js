import React from "react";
import BadRequest from "../../components/organisms/Error/BadRequest";

export default function (props) {
  return (
    <BadRequest
      icon="dizzy"
      title="Có lỗi xảy ra với truy cập của bạn"
      instruction="Liên kết bạn đang truy cập có thể tạm thời bị lỗi, hãy thử lại hoặc quay lại vào lúc khác"
      actionUrl="/"
      actionText="Quay về trang chủ"
    />
  );
}
