   M S B A M L   `   `   `  ����E  AMilestone1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Milestone1
Milestone1  5   	       Milestone1.Reviews   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Milestone1     5   	     � Title$  Reviews��5   	   $��500         ��6   $
��1000��5	      .��5
      � -   5         �Name$ ReviewWindow��5
       � ColumnDefinitions 5   
   �� 5      $	��61* �6   �� 5      $
��933* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 
reviewGrid��5      $��1��6'   $
��Left=�68   $	��312��6S   $��28.2,148,0,0q�6`   $	��Top=�6v   $	��818��6�   ��. $
��Auto=�5      $
��Auto=�5      �� 5   
   -   5         ��Name$ reviewTextBox��5      $��1��6)   $
��Left=�69   $��74��6T   $��28.2,34,0,0q�6`    �� TextWrapping$
 Wrap=�6u   $��<<Review Text>>��6�   $	��Top=�6�   $	��546��6�   �� 5   
   -   5         ��Name$ ratingDropDown��5      $��1��6+   $
��Left=�6;   $��769.2,42,0,0q�6V   $	��Top=�6l   $	��110��6�   $��31��6�   .@�5      �� $��1��6   �� 5      $��2��6   �� 5      $��3��6   �� 5      $��4��6   �� 5      $��5��6   5      �� 5   
   -   5         ��Name$ addReviewButton��5      $��
Add Review��6*   $��1��6?   $
��Left=�6O   $��723.2,103,0,0q�6j   $	��Top=�6�   $	��179��6�   $��40��6�   6�   �� 5   
   -   5         ��Name$ ratingLabel��5      $��Rating:��6%   $��1��67   $
��Left=�6G   $��769.2,16,0,0q�6b   $	��Top=�6x   $��86��6�   �� 5   
   -   5        $ reviewidTextBox��5      $��1��6+   $
��Left=�6;   $��23��6V   $��625.2,50,0,0q�6b   $
 Wrap=�6x   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $ reviewidLabel��5      $��ReviewID��6'   $��1��6:   $
��Left=�6J   $��625.2,19,0,0q�6e   $	��Top=�6{   $��58��6�   5      5       