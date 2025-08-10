export interface Category {
  id: number;
  name: string;
}

export interface Shop {
  id: number;
  name: string;
}

export interface Purchase {
  id: number;
  shop: Shop;
  billCost: number;
  category: Category;
  date: string;
  note: string;
}
