import React, { useEffect } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Product from "../../components/templates/Product";
import { UrlConstant } from "../../utils/Constants";
import { useQuery, useMutation } from "@apollo/client";
import { productQueries } from "../../graphql/fetching/queries";
import { productMutations } from "../../graphql/fetching/mutations";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { pageNumber } = params;
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(
    productQueries.GET_PRODUCTS,
    {
      variables: {
        criterias: {
          page: pageNumber ? parseInt(pageNumber) : 1,
        },
      },
    }
  );

  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  useEffect(() => {
    if (state.type === "PRODUCT_UPDATE" || state.type === "PRODUCT_DELETE") {
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
    if (product.pictures && product.pictures.length > 0) {
      const picture = product.pictures[0];
      if (picture.pictureId > 0) {
        product.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${picture.pictureId}`;
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

    if (product.farms) {
      product.farms = product.farms.map((pf) => {
        let productFarm = { ...pf };
        productFarm.url = `/farms/${pf.id}`;
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

  const onOpenDeleteConfirmation = (e) => {
    const { title, innerModal, message, id } = e;
    dispatch("OPEN_MODAL", {
      data: {
        title: title,
        children: message,
        id: id,
      },
      execution: { onDelete: onDelete },
      options: {
        isOpen: true,
        innerModal: innerModal,
        position: "fixed",
      },
    });
  };

  const onDelete = (id) => {
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      refetch();
    });
  };

  return (
    <DefaultLayout>
      <Product
        onOpenDeleteConfirmation={onOpenDeleteConfirmation}
        products={products}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={page}
      />
    </DefaultLayout>
  );
});
