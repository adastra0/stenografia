   M S B A M L   `   `   `  ���� J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1       WpfApplication1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:WpfApplication1       � Title$  
MainWindow��$	��600��$	��800��.��� .+�]� $	��Top=�$��Stretch=�.+�+  %System.Windows.Controls.Ribbon.Ribbon     ApplicationMenu :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  .@�>  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -   $��About��.@�.  (System.Windows.Controls.Ribbon.RibbonTab  $
��Home��   KeyTip$ H��.@�0  *System.Windows.Controls.Ribbon.RibbonGroup  -     Name$
 Home��$
��Home��.@�5  /System.Windows.Controls.Ribbon.RibbonMenuButton     LargeImageSource$ Images\icons8-open.png��   Label$
 Open��   KeyTip$ V��.@�3  -System.Windows.Controls.Ribbon.RibbonMenuItem  -   $��Bitmap��   KeyTip$ B��  -   $��File to hide��$ F��  $ Images\icons8-save.png��$
 Save��$ X��.@�  -   $��Encoded bitmap��$ B��  -   $��Decoded file��$ F��  -   $ Coding��$��Coding��   KeyTip$ C��.@�  -   $��Encode��$ E��  -	   $��Decode��$ D��  -
   $ 
Parameters��$��
Parameters��$y�Hidden=�.@��� -   	 ��Name$	 label_r��
 �� ContentStringFormat$
 	R = {}{0}������  �� ElementName$ slider_r�� �� Path$ Value��� -    ��Name$ slider_r��$��60��$��30�� �� IsSnapToTickEnabled .  �� TickFrequency$ 1Q� � Maximum$ 8Q� � Minimum$ 1Q� �� TickPlacement$ TopLeft=�
���  ��$��2q� J� RenderTransformOrigin$ 0.5,0.5&�$��0q�z�V� .~��� �� ��  �� Angle$	 270Q�T� �� -   $	 label_g��$
 
R = {0}{1}��$��0q����� $ slider_g��$ Value��� -   $ slider_g��$��60��$��30�� . $ 1Q�$ 8Q�$ 1Q�$ TopLeft=�
��� � �$��2q�$��0q�$ 0.5,0.5&�z�V� .~��� �� �� $	 270Q�T� �� -   $	 label_b��$
 	R = {}{0}��$��0q����� $ slider_b��$ Value��� -   $ slider_b��$��60��$��30�� . $ 1Q�$ 8Q�$ 1Q�$ TopLeft=�
����  �$��2q�$��0q�$ 0.5,0.5&�z�V� .~��� �� �� $	 270Q�T�   -   $
 Info��$
��Info��$y�Hidden=�.@��� -   $	 labelCapacity��$��Pojemność bitmapy: ���� -   $	 labelFileSize��$��Rozmiar pliku: ��]� $
��Left=�$	��Top=�$��
20,158,0,0q�$	��360��$	��360��
���   �.+��� -   $
��img1��$	��Top=�$��Stretch=�$��Stretch=�]� $
��Left=�$	��Top=�$��415,158,0,0q�$	��360��$	��360��
���   �.+��� -   $
��img2��$	��Top=�$��Stretch=�$��Stretch=�