   M S B A M L   `   `   `  ����  BouncingBall    BouncingBall.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Bouncing Ball��5   	   $	��500��6   $	��765��6,   .��5
      � .+�5   
   �� -   5        $��ScoreCanvas��5      
�������$	��100��6;   $	��765��6H   $	��Top=�6T   .+�5      �� -   5        $��TimeBox��5      $��32��6%    �� Left$	 107��61    �� TextWrapping$
 Wrap=�6C   
 �� Top$ 36��6�   $��40��6�   ��6W   ��  �� Path$ 	Gametimed� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5      -   5        $��ScoreBox��5      $��32��6&   $	 269��62   $
 Wrap=�6D   $ 36��6�   $��40��6�   ��6X   �� $ Scored�$ PropertyChanged=�6�   �� 5      $
��Wrap=�6   $	 107��6,   $ 20��6>   $��Time Elapsed��6N   �� 5      $
��Wrap=�6   $Y��SPress R to restart, T to reset the bricks, S to start and stop the ball, G to exit.��6,   $	 410��6�   $ 36��6�   $��32��6�   $	��264��6�   5      �� 5   
   -   5        $��
BallCanvas��5      $��Bottom=�5      $	��368��5      $	��765��5      
�������.+�5      A�  5      �� *ballCanvasTop+ 5      �� *ballCanvasLeft+��5      �� *	BallWidth+��5      �� *
BallHeight+��5      
� 5       
� GradientOrigin$ 	0.75,0.25&�6*   .��5      �  � Color$ Yellow��6'    � Offset$	 0.0Q�66   � 5      $ Orange��6'   $	 0.5Q�66   � 5       $	 Red��6'   $	 1.0Q�63   5!      5#      �� 5$      
���   � 5%      �� *paddleCanvasTop+ 5&      �� *paddleCanvasLeft+��5(      �� *paddleHeight+��5)      �� *paddleWidth+5*      �� 5+      -   5        	 ��Name$	 MyItemsControl��5+      $	��330��64   $��Center=�6@   $	��Top=�6]   $��0,10,0,0q�6u   ��5,      �� 5-      .R�5.      �� .+�5/      �� 
���  ����51      �� *	BrickFill+��53      �� *BrickHeight+��54      �� *
BrickWidth+ 55      �� *BrickCanvasTop+ 56      �� *BrickCanvasLeft+y�57      �� *BrickVisible+58      59      5A      5C      5D      �� 5E   
   $
��Left=�6   $
��Wrap=�6/   $��Score��6C   $	��Top=�6P   $��
257,20,0,0q�6h   5F      5G      