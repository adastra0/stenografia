   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1  5   	        WpfApplication1.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApplication1     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��800��6)   .��5	      � .+�5
   
   ]� $	��Top=�6   $��Stretch=�6,   .+�5      +  %System.Windows.Controls.Ribbon.Ribbon     ApplicationMenu 5      :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5      .@�5      >  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -   5        $��About��5   4   6C   5      .@�5      .  (System.Windows.Controls.Ribbon.RibbonTab  $
��Home��6      KeyTip$ H��6*   .@�5      0  *System.Windows.Controls.Ribbon.RibbonGroup  -   5          Name$
 Home��5   "   $
��Home��60   .@�5      5  /System.Windows.Controls.Ribbon.RibbonMenuButton     LargeImageSource$ Images\icons8-open.png��6+      Label$
 Open��6U      KeyTip$ V��6b   .@�5      3  -System.Windows.Controls.Ribbon.RibbonMenuItem  -   5        $��Bitmap��5   -      KeyTip$ B��6=   6H     5      -   5        $��File to hide��5   -   $ F��6C   6N   5        5      $ Images\icons8-save.png��6+   $
 Save��6U   $ X��6b   .@�5        -   5        $��Encoded bitmap��5   -   $ B��6E   6P     5      -   5        $��Decoded file��5   -   $ F��6C   6N   5      5        5      -   5        $ Coding��5   "   $��Coding��62      KeyTip$ C��6B   .@�5        -   5        $��Encode��5   )   $ E��69   6U     5       -	   5        $��Decode��5    )   $ D��69   6U   5!        5#      -
   5        $ 
Parameters��5#   "   $��
Parameters��66   $y�Hidden=�6J   .@�5%      �� -   5        	 ��Name$	 label_r��5%       
 �� ContentStringFormat$
 	R = {}{0}��6f   ��61   ��  �� ElementName$ slider_r�� �� Path$ Value�6f   �� 5&      -   5         ��Name$ slider_r��5&   !   $��60��63   $��30��6>    �� IsSnapToTickEnabled .  �� TickFrequency$ 1Q�6e    � Maximum$ 8Q�6w    � Minimum$ 1Q�6�    �� TickPlacement$ TopLeft=�6�   
���  ��$��2q�6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   $��0q�6  z�5'      V� 5(   "   .~�5)   &   �� �� 5*   &   �� 5+   &    �� Angle$	 270Q�66   T� 5,   &   5-   #   5/      �� 51      -   5        $	 label_g��51       $
 
R = {0}{1}��6f   $��0q�6�   ��61   �� $ slider_g��$ Value�6�   �� 52      -   5        $ slider_g��52   !   $��60��63   $��30��6>    . $ 1Q�6e   $ 8Q�6w   $ 1Q�6�   $ TopLeft=�6�   
��� � �$��2q�6�   $��0q�6�   $ 0.5,0.5&�6�   z�53      V� 54   "   .~�55   &   �� �� 56   &   �� 57   &   $	 270Q�66   T� 58   &   59   #   5;      �� 5=      -   5        $	 label_b��5=       $
 	R = {}{0}��6f   $��0q�6�   ��61   �� $ slider_b��$ Value�6�   �� 5>      -   5        $ slider_b��5>   !   $��60��63   $��30��6>    . $ 1Q�6e   $ 8Q�6w   $ 1Q�6�   $ TopLeft=�6�   
����  �$��2q�6�   $��0q�6�   $ 0.5,0.5&�6�   z�5?      V� 5@   "   .~�5A   &   �� �� 5B   &   �� 5C   &   $	 270Q�66   T� 5D   &   5E   #   5G      5H        5I      -   5        $
 Info��5I   "   $
��Info��60   $y�Hidden=�6>   .@�5J      �� -   5        $	 labelCapacity��5J       $��Pojemność bitmapy: ��67   �� 5K      -   5        $	 labelFileSize��5K       $��Rozmiar pliku: ��67   5L      5M      5N      5O      ]� 5P   
   $
��Left=�6   $	��Top=�6/   $��
20,158,0,0q�6G   $	��360��6[   $	��360��6h   
���   �.+�5Q      �� -   5        $
��img1��5Q      $	��Top=�6    $��Stretch=�65   $��Stretch=�6S   5R      ]� 5S   
   $
��Left=�6   $	��Top=�6/   $��415,158,0,0q�6G   $	��360��6\   $	��360��6i   
���   �.+�5T      �� -   5        $
��img2��5T      $	��Top=�6    $��Stretch=�65   $��Stretch=�6S   5U      5V      5W      