   M S B A M L   `   `   `  ���� H  DGrocerySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:GrocerySystemGrocerySystem      GrocerySystem.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:GrocerySystem       � Title$  GroceryStoreSystem��$	��650��$	��830��.��� $	��630��$	��830�� � RowDefinitions �� $��90 ��� �� $��50 �.+�� $��0�� �� ��  � ColumnDefinitions �� �� �� �� .+��� -   $��lblUsername��$��0��$��2��$��Right=�$��0,0,49,2q�$��Bottom=�.��	Username:�� -   $��lblPassword��$��1��$��2��$��Right=�$��	0,0,49,16q�$��Bottom=�.��	Password:�� -   $��txtUsername��$��20��$	��150��$��0��$��2��$��155,21,100,4q�$��2��G� -   $��txtPassword��$��20��$	��150��$��1��$��2��$��155,4,100,21q�$��2���� -   $��btnLogin��$��0��$��2��$��75��$��20��$��3��$��106,35,26,35q�$��Log In���� $��16,18,17,17q�$��/Icon/jwslogo.png��$��2��$
��Fill=��� -   $��
statusMain��$��20 0 0 0q�$��
10,0,0,457q�$	��797��$��33��
���   ����	#FFECECEC$��16(� J� RenderTransformOrigin$ 0.5,0.5&�$
��Bold"�$
��Left=�$��Center=�$��1��
���   �.@��� $��Trebuchet MS)�$��Italic$�$��16(�$��Normal"�.��Please Log Into Your account.�� -    ��Name$ 
tabsetMain��$��1��$��10,55,23,24q�$��2��.@��� -    ��Name$ tabRentEquipment��$��	Inventory��.��� 
������� �� �� $��30 �.+�&   System.Windows.Controls.DataGrid  -	     Name$ grInventory��$��0��� $��1�� �� �� �� �� �� �� .+��� -
    ��Name$ btnItemDetails��$��Details��$��17,6,0,4q�$��1���� -   $ btnEditItem��$��	Edit Item��$��17,6,0,4q�$��2���� -   $ 
btnAddItem��$��Add New Item��$��17,6,0,4q�$��3���� -   $ btnDiscontinueItem��$��Discontinue Item��$��17,6,1,4q�$��4���� -   $ tabDelivery��$��Grocery Item���� -   $ 	tabPickup��$��Grocery Item���� -   $ tabInspection��$��Grocery Item���� -   $ tabPrepEquipment��$��Grocery Item���� -   $ tabMaintenance��$��Grocery Item���� -   $ tabInventory��$��Grocery Item��.��� 
������� �� �� $��30 �.+�  -   $ dgInventory��$��0��� $��1�� �� �� �� �� �� �� .+��� -   $ btnEquipmentDetails��$��Details��$��
129,5,20,5q�$��1��$��2���� -   $ btnEditEquipment��$��Edit Details��$��
129,5,20,5q�$��2��$��2���� -   $ btnAddEquipment��$��Add Equipment��$��
129,5,20,5q�$��3��$��2���� -   $ btnDeactivateEquipment��$��
Deactivate��$��
129,5,20,5q�$��4��$��2���� $��10,55,23,24q�$��
cover.jpeg��$
��Fill=�$��2��$��1��