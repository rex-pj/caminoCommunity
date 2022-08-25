import React from "react";
import NotFound from "../../components/organisms/Error/NotFound";
import { PromptLayout } from "../../components/templates/Layout";

export default function (props) {
  return (
    <PromptLayout>
      <NotFound
        icon="unlink"
        title="Không tìm thấy trang này"
        instruction="Liên kết bạn đang truy cập có thể bị gỡ hoặc không tồn tại"
        actionUrl="/"
        actionText="Quay về trang chủ"
      />
    </PromptLayout>
  );
}
