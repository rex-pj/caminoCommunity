import * as React from "react";
import BadRequest from "../../components/organisms/Error/BadRequest";
import { PromptLayout } from "../../components/templates/Layout";

type Props = {};

const Index = (props: Props) => {
  return (
    <PromptLayout>
      <BadRequest
        icon="dizzy"
        title="Có lỗi xảy ra với truy cập của bạn"
        instruction="Liên kết bạn đang truy cập có thể tạm thời bị lỗi, hãy thử lại hoặc quay lại vào lúc khác"
        actionUrl="/"
        actionText="Quay về trang chủ"
      />
    </PromptLayout>
  );
};

export default Index;
