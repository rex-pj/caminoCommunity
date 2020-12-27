import React, { useEffect } from "react";
import Product from "../../components/templates/Product";
import { UrlConstant } from "../../utils/Constants";
import { useQuery } from "@apollo/client";
import { productQueries } from "../../graphql/fetching/queries";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { pageNumber, pageSize } = params;
  const [state] = useStore(false);
  const { loading, data, error, refetch } = useQuery(
    productQueries.GET_PRODUCTS,
    {
      variables: {
        criterias: {
          page: pageNumber ? parseInt(pageNumber) : 1,
          pageSize: pageSize ? parseInt(pageSize) : 10,
        },
      },
    }
  );

  useEffect(() => {
    if (state.type === "PRODUCT" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  if (loading || !data) {
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { products: productsData } = data;
  const { collections } = productsData;
  const products = collections.map((item) => {
    let product = { ...item };
    product.url = `${UrlConstant.Product.url}${product.id}`;
    if (product.thumbnails && product.thumbnails.length > 0) {
      const thumbnail = product.thumbnails[0];
      if (thumbnail.pictureId > 0) {
        product.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.pictureId}`;
      }
    }

    product.creator = {
      createdDate: item.createdDate,
      profileUrl: `/profile/${item.createdByIdentityId}`,
      name: item.createdBy,
    };

    if (item.createdByPhotoCode) {
      product.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
    }

    if (product.productFarms) {
      product.productFarms = product.productFarms.map((pf) => {
        let productFarm = { ...pf };
        productFarm.url = `/farms/${pf.farmId}`;
        return productFarm;
      });
    }

    return product;
  });

  const baseUrl = "/products";
  const { totalPage, filter } = productsData;
  const { page } = filter;

  const breadcrumbs = [
    {
      isActived: true,
      title: "Product",
    },
  ];

  return (
    <Product
      products={products}
      breadcrumbs={breadcrumbs}
      totalPage={totalPage}
      baseUrl={baseUrl}
      currentPage={page}
    />
  );
});
