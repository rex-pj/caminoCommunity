import * as React from "react";
import BadRequest from "../../components/organisms/Error/BadRequest";
import { PromptLayout } from "../../components/templates/Layout";
import { Helmet } from "react-helmet-async";

type Props = {};

const Index = (props: Props) => {
  const metaTitle = `Oops! ${"| Nông Trại LỒ Ồ"}`;
  return (
    <>
      <Helmet>
        {metaTitle ? <title>{metaTitle}</title> : null}
        <meta name="robots" content="noindex,nofollow" />
      </Helmet>
      <PromptLayout>
        <BadRequest icon="dizzy" title="Có lỗi xảy ra với truy cập của bạn" instruction="Liên kết bạn đang truy cập có thể tạm thời bị lỗi, hãy thử lại hoặc quay lại vào lúc khác" actionUrl="/" actionText="Quay về trang chủ" />
      </PromptLayout>
    </>
  );
};

export default Index;
